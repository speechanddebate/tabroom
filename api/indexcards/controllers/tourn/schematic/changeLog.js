import objectify from '../../../helpers/objectify';

export const getRoundLog = {
	GET: async (req, res) => {

		const roundQuery = `
			select
				cl.id, cl.tag, cl.description, cl.count,
				CONVERT_TZ(cl.timestamp, "+00:00", tourn.tz) timestamp,
				person.id person, person.email, person.first, person.last,
				round.id round, round.name round_name, round.label round_label

			from (change_log cl, round, event, tourn)

				left join person on cl.person = person.id

			where round.id = :roundId
				and round.id = cl.round
				and round.event = event.id
				and event.tourn = tourn.id
		`;

		const rawRoundLogs = await req.db.sequelize.query(roundQuery, {
			replacements: { roundId: req.params.roundId },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		const panelQuery = `
				select
				cl.id, cl.tag, cl.description, cl.count,
				CONVERT_TZ(cl.timestamp, "+00:00", tourn.tz) timestamp,
				person.id person, person.email, person.first, person.last,
				round.id round, round.name round_name, round.label round_label,
				panel.id panel, panel.letter letter

			from (change_log cl, round, event, tourn, panel)

				left join person on cl.person = person.id

			where round.id = :roundId
				and round.id = panel.round
				and panel.id = cl.panel
				and round.event = event.id
				and event.tourn = tourn.id
		`;

		const rawPanelLogs = await req.db.sequelize.query(panelQuery, {
			replacements: { roundId: req.params.roundId },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		const roundLogs = [...rawPanelLogs, ...rawRoundLogs];

		res.status(200).json(objectify(roundLogs));
	},
};

export default getRoundLog;
