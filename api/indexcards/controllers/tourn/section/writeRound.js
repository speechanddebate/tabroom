// Takes a created round object with sections and writes it into the database

export const writeRound = async (db, round) => {

	await db.panel.destroy({ where: { round: round.id } });
	let letter = 1;

	if (round.type === 'debate') {

		round.sections.forEach( async (section) => {

			const judge = section.j || 0;
			round.panels = [];

			if (section.b) {

				const panel = await db.panel.create({
					round   : round.id,
					letter,
					flight  : 1,
					bye     : 1,
				});

				const ballot = await db.ballot.create({
					panel : panel.id,
					side  : 1,
					entry : section.b,
					audit : 1,
				});

				panel.ballots = [ballot];
				round.panels.push(panel);

			} else if (section.a && section.n) {

				const panel = await db.panel.create({
					round   : round.id,
					letter,
					flight  : 1,
				});

				const aff = await db.ballot.create({
					panel : panel.id,
					side  : 1,
					entry : section.a,
					judge,
				});

				const neg = await db.ballot.create({
					panel : panel.id,
					side  : 2,
					entry : section.n,
					judge,
				});

				panel.ballots = [aff, neg];
				round.panels.push(panel);
			}

			letter++;
		});

	} else if (round.type === 'pf') {

		round.sections.forEach( async (section) => {
			if (section.length === 1) {
				const panel = await db.panel.create({
					round   : round.id,
					letter,
					flight  : 1,
					bye     : 1,
				});

				const ballot = await db.ballot.create({
					panel : panel.id,
					side  : 1,
					entry : section.b,
					audit : 1,
				});

				panel.ballots = [ballot];
				round.panels.push(panel);

			} else {

				const panel = await db.panel.create({
					round   : round.id,
					letter,
					flight  : 1,
				});

				let side = 1;
				panel.ballots = [];

				section.forEach( async (entry) => {
					const ballot = await db.ballot.create({
						panel : panel.id,
						side,
						entry,
					});
					panel.ballots.push(ballot);
					side++;
				});
				round.panels.push(panel);
			}
			letter++;
		});

	} else {

		round.sections.forEach( async (section) => {

			const panel = await db.panel.create({
				round   : round.id,
				letter,
				flight  : 1,
			});

			let speakerorder = 1;
			panel.ballots = [];

			section.forEach( async (entry) => {
				const ballot = await db.ballot.create({
					panel : panel.id,
					speakerorder,
					entry,
				});
				panel.ballots.push(ballot);
				speakerorder++;
			});
			round.panels.push(panel);

			letter++;
		});
	}

	return round;

};

export default writeRound;
