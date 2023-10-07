import moment from 'moment-timezone';
import { roundCheck } from '../../../../helpers/auth.js';

export const scheduleAutoFlip = async (roundId, req, res) => {

	const permOK = await roundCheck(req, res, roundId);
	if (!permOK) {
		return;
	}

	const roundData = await req.db.sequelize.query(`
		select round.id, round.flighted, round.type, round.published,
			round.flighted flights,
			round.start_time roundstart, timeslot.start,
			event.id event,
			flip_autopublish.value flip_autopublish,
			flip_before_start.value flip_before_start,
			flip_split_flights.value flip_split_flights,
			flight_offset.value flight_offset,
			flip_published.value flip_published,
			no_side_constraints.value no_side_constraints,
			tourn.tz

		from (round, event, timeslot, tourn)

		left join event_setting flip_autopublish
			on flip_autopublish.event = event.id
			and flip_autopublish.tag = 'flip_autopublish'

		left join event_setting flip_before_start
			on flip_before_start.event = event.id
			and flip_before_start.tag = 'flip_before_start'

		left join event_setting flight_offset
			on flight_offset.event = event.id
			and flight_offset.tag = 'flight_offset'

		left join event_setting flip_split_flights
			on flip_split_flights.event = event.id
			and flip_split_flights.tag = 'flip_split_flights'

		left join round_setting flip_published
			on flip_published.round = round.id
			and flip_published.tag = 'flip_published'

		left join event_setting no_side_constraints
			on no_side_constraints.event = event.id
			and no_side_constraints.tag = 'no_side_constraints'

		where round.id = :roundId

			and round.event = event.id
			and round.timeslot = timeslot.id
			and event.tourn = tourn.id
			and not exists (
				select es.id
					from event_setting es
				where es.event = event.id
					and es.tag = 'sidelock_elims'
			)

			and exists (
				select fo.id
					from event_setting fo
				where fo.event = event.id
					and fo.tag = 'flip_online'
			)
			and not exists (
				select fa.id
					from autoqueue fa
				where fa.round = round.id
					and fa.tag like 'flip_%'
			)
		`, {
		replacements : { roundId },
		type         : req.db.sequelize.QueryTypes.SELECT,
	});

	if (roundData) {
		roundData.forEach( async (round) => {

			if (!round.no_side_constraints
				&& ( round.type !== 'elim'
					&& round.type !== 'final'
					&& round.type !== 'runoff'
				)
			) {
				return;
			}

			if (!round.roundstart) {
				round.roundstart = round.start;
			}

			const flipAt = {};
			let flights = [];

			if (round.flip_split_flights) {
				flights = [...Array(round.flights).keys()];
			} else {
				flights = [0];
			}

			if (round.flip_before_start) {
				flights.forEach( (tick) => {
					const flight = tick + 1;
					flipAt[flight] = moment(round.roundstart)
						.add( (parseInt(tick * round.flight_offset) - parseInt(round.flip_before_start)), 'minutes');
				});
			} else if (round.flip_autopublish) {
				flights.forEach( (tick) => {
					const flight = tick + 1;
					flipAt[flight] = moment()
						.add(parseInt((tick * round.flight_offset) + parseInt(round.flip_autopublish)), 'minutes');
				});
			}

			flights.forEach( async (tick) => {
				const flight = tick + 1;

				if (round.flip_split_flights) {
					await req.db.autoqueue.create({
						tag        : `flip_${flight}`,
						round      : round.id,
						active_at  : flipAt[flight],
						created_at : Date(),
					});
				} else {
					await req.db.autoqueue.create({
						tag        : `flip`,
						round      : round.id,
						active_at  : flipAt[flight],
						created_at : Date(),
					});
				}
			});
		});
	}
};

export default scheduleAutoFlip;
