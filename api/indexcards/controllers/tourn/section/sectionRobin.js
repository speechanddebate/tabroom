// Assigning a round robin.  For now this is used only for defined pattern
// round robins that have preset patterns in the global settings.

// import { showDateTime } from '../../../helpers/common';

export const sectionRobin = {
	POST: async (req, res) => {
		const db = req.db;
		const division = await db.summon(db.event, req.params.event_id);

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

			// Clear the old sections if any
			await db.panel.destroy({ where: { round: round.id } });

			Object.keys(rrPattern[round.name]).forEach( async (letter) => {

				const section = rrPattern[round.name][letter];
				console.log(`Creating round ${round.name} section ${letter}`);

				if (section.b) {

					const panel = await db.panel.create({
						round   : round.id,
						letter,
						flight  : 1,
						publish : 0,
						room    : 0,
						bye     : 1,
					});

					await db.ballot.create({
						entry        : positions[section.b].entry,
						judge        : 0,
						panel        : panel.id,
						side         : 1,
						speakerorder : 0,
						audit        : 1,
					});

				} else {

					if (!positions[section.j].judge) {
						positions[section.j].judge = 0;
					}

					const panel = await db.panel.create({
						round   : round.id,
						letter,
						flight  : 1,
						publish : 0,
						room    : 0,
					});

					await db.ballot.create({
						entry        : positions[section.a].entry,
						judge        : positions[section.j].judge,
						side         : 1,
						speakerorder : 0,
						audit        : 0,
						panel        : panel.id,
					});

					await db.ballot.create({
						entry        : positions[section.n].entry,
						judge        : positions[section.j].judge,
						side         : 2,
						speakerorder : 0,
						audit        : 0,
						panel        : panel.id,
					});
				}
			});
		});

		res.status(200).json({ error: false, message: `${rounds.length} paired for the round robin`, refresh: true });
	},
};

export const sectionPrelim = {
	GET: async (req, res) => {
		const db = req.db;
		const round = await db.summon(db.round, req.params.round_id);

		res.status(200).json(round);

	},
};
