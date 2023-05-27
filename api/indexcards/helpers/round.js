// Common helper functions that attach to rounds & schematics
import db from './db';
import objectify from './objectify';

export const sidelocks = async (roundId) => {

	const sidelockQuery = `
		select
			section.id,
			count(other.id) count,
				neg_bo.side aff_ok,
				aff_bo.side neg_ok

		from panel section, ballot aff_b, ballot neg_b, entry aff_e, entry neg_e,
			panel other, ballot aff_bo, ballot neg_bo

		where section.round = :roundId
			and section.id = aff_b.panel
			and aff_b.side = 1
			and aff_b.entry = aff_e.id

			and section.id = neg_b.panel
			and neg_b.side = 2
			and neg_b.entry = neg_e.id

			and section.round != other.round

			and other.id = aff_bo.panel
			and aff_bo.entry = aff_e.id

			and other.id = neg_bo.panel
			and neg_bo.entry = neg_e.id
	`;

	const sideLocks = await db.sequelize.query(sidelockQuery, {
		replacements : { roundId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	if (sideLocks) {
		return objectify(sideLocks);
	}
};

export const flightTimes = async (roundId) => {

	const roundSettings = await db.sequelize.query(`
		select
			round.id, round.name, round.start_time, round.flighted, round.type,
			tourn.tz,
			flight_offset.value flight_offset,
			prelim_decision_deadline.value prelim_deadline,
			elim_decision_deadline.value elim_deadline

		from (round, event, tourn)

			left join event_setting flight_offset
				on flight_offset.event = event.id
				and flight_offset.tag = 'flight_offset'

			left join event_setting prelim_decision_deadline
				on prelim_decision_deadline.event = event.id
				and prelim_decision_deadline.tag = 'prelim_decision_deadline'

			left join event_setting elim_decision_deadline
				on elim_decision_deadline.event = event.id
				and elim_decision_deadline.tag = 'elim_decision_deadline'

		where round.id = :roundId
			and round.event = event.id
			and event.tourn = tourn.id

	`, { replacements: { roundId },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const round = roundSettings.shift();
	const times = { tz: round.tz };
	const roundStart = new Date(round.start_time);

	if (round.flighted < 1) {
		round.flighted = 1;
	}

	for (let f = 1; f <= round.flighted; f++) {

		times[f] = { };

		if (round.flight_offset) {
			times[f].start = new Date(roundStart.getTime()
				+ ((f - 1) * round.flight_offset * 60000)
			);

		} else {
			times[f].start = new Date(roundStart.getTime());
		}

		if (round.type === 'final' || round.type === 'elim' || round.type === 'runoff') {

			if (round.elim_deadline) {
				if (round.flight_offset) {

					times[f].deadline = new Date(
						roundStart.getTime()
						+ ((f - 1) * round.flight_offset * 60000)
						+ (round.elim_deadline * 60000)
					);

				} else {

					times[f].deadline = new Date(
						roundStart.getTime()
						+ (round.elim_deadline * 60000)
					);
				}
			}
		} else {

			if (round.prelim_deadline) {
				if (round.flight_offset) {

					times[f].deadline = new Date(
						roundStart.getTime()
						+ ((f - 1) * round.flight_offset * 60000)
						+ (round.prelim_deadline * 60000)
					);

				} else {
					times[f].deadline = new Date(
						roundStart.getTime()
						+ (round.prelim_deadline * 60000)
					);
				}
			}
		}
	}

	return times;
};

export default sidelocks;
