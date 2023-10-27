// Assigning a round robin.  For now this is used only for defined pattern
// round robins that have preset patterns in the global settings.

// import { showDateTime } from '../../../helpers/common';
import { writeRound } from './writeRound';

export const sectionRobin = {
	POST: async (req, res) => {
		const db = req.db;
		const division = await db.summon(db.event, req.params.eventId);

		const entries = await db.entry.findAll({
			where: { event: division.id, active: 1 },
			raw: true,
		});

		const rrTag = `round_robin_${entries.length}`;

		const rrSetting = await db.tabroomSetting.findOne({
			where: { tag: rrTag },
			raw: true,
		});

		const rrPattern = JSON.parse(rrSetting.value_text);

		if (!rrPattern) {
			res.status(200).json({ error: true, message: `No pattern found for ${entries.length}` });
		}

		const rounds = await db.round.findAll({
			where: { event: division.id },
			raw: true,
		});

		if (rounds.length !== rrPattern.rounds) {
			res.status(200).json({ error: true, message: `Incorrect round count for pattern. ${rrPattern.rounds} rounds required` });
		}

		const judges = await db.judge.findAll({
			where: { category: division.category, active: 1 },
			raw: true,
		});

		const positions = {};
		let index = 1;

		entries.sort(() => Math.random() - 0.5).forEach( (entry) => {

			positions[index] = {};
			positions[index].entry = entry.id;
			judges.forEach( (judge) => {
				if (judge.school === entry.school) {
					positions[index].judge = judge.id;
				}
			});
			index++;
		});

		rounds.forEach( async (round) => {

			round.sections = [];

			Object.keys(rrPattern[round.name]).forEach( async (letter) => {

				const template = rrPattern[round.name][letter];
				const section = {};

				if (template.b) {
					section.b = positions[template.b].entry;
				} else {
					section.j = positions[template.j].judge;
					section.a = positions[template.a].entry;
					section.n = positions[template.n].entry;
				}
				round.sections.push(section);
			});

			round.type = 'debate';
			await writeRound(db, round);
		});

		res.status(200).json({ error: false, message: `${rounds.length} paired for the round robin`, refresh: true });
	},
};

export const sectionPrelim = {
	GET: async (req, res) => {
		const db = req.db;
		const round = await db.summon(db.round, req.params.roundId);
		res.status(200).json(round);
	},
};
