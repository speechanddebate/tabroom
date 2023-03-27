import db from './db';

export const followers = async (replacements, options = { all: true }) => {

	let whereLimit = '';

	if (replacements.panelId) {
		whereLimit = ` where panel.id = :panelId `;
		delete replacements.roundId;
	} else if (replacements.roundId) {
		whereLimit = ` where panel.round = :roundId `;
	} else {
		return { error: true, message: `No round or section to blast sent` };
	}

	const blastMe = {
		text  : [],
		email : [],
		error : false,
	};

	if (options.all || options.entries || options.speakerorder) {

		let entryPeopleQuery = `
			select
				person.id, person.email, person.phone, person.provider
			from (panel, person, ballot, entry, entry_student es, student)
			${whereLimit}
				and ballot.panel = panel.id
				and ballot.entry = entry.id
				and entry.active = 1
				and entry.id = es.entry
				and es.student = student.id
				and student.person = person.id
				and person.no_email = 0
		`;

		if (options.speaker) {
			entryPeopleQuery += `
				and ballot.speakerorder = :speakerOrder
			`;
			replacements.speakerOrder = options.speakerOrder;
		}

		const rawEntryPeople = await db.sequelize.query(entryPeopleQuery, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawEntryPeople.forEach( (person) => {

			if (person.provider && person.phone) {
				blastMe.text.push(`${person.phone}@${person.provider}`);
			}

			if (person.email) {
				blastMe.email.push(`${person.email}`);
			}
		});
	}

	if (options.all || options.judges || options.speakerorder) {

		let judgePeopleQuery = `
			select
				person.id, person.email, person.phone, person.provider
			from (person, ballot, judge, panel)
			${whereLimit}
				and ballot.panel = panel.id
				and ballot.judge = judge.id
				and judge.person = person.id
				and person.no_email = 0
		`;

		if (options.unconfirmed) {
			judgePeopleQuery += `
				and (ballot.audit = 1 OR ballot.audit IS NULL)
			`;
		} else if (options.unstarted) {
			judgePeopleQuery += `
				and (ballot.judge_started IS NULL)
			`;
		}

		const rawJudgePeople = await db.sequelize.query(judgePeopleQuery, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawJudgePeople.forEach( (person) => {
			if (person.provider && person.phone) {
				blastMe.text.push(`${person.phone}@${person.provider}`);
			}
			if (person.email) {
				blastMe.email.push(`${person.email}`);
			}
		});
	}

	if (!options.no_followers && !options.speakerorder) {

		if (options.all || options.entries) {

			const entryFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider
				from (person, ballot, entry, follower, panel)
				${whereLimit}
					and ballot.panel = panel.id
					and ballot.entry = entry.id
					and entry.active = 1
					and entry.id = follower.entry
					and follower.person = person.id
					and person.no_email = 0
			`;

			const rawEntryFollowers = await db.sequelize.query(entryFollowersQuery, {
				replacements,
				type: db.sequelize.QueryTypes.SELECT,
			});

			rawEntryFollowers.forEach( (person) => {

				if (person.provider && person.phone) {
					blastMe.text.push(`${person.phone}@${person.provider}`);
				}

				if (person.email) {
					blastMe.email.push(`${person.email}`);
				}
			});
		}

		if (options.all || options.judges) {

			const judgeFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider
				from (person, ballot, follower, panel)
				${whereLimit}
					and ballot.panel = panel.id
					and ballot.judge = follower.judge
					and follower.person = person.id
					and person.no_email = 0
			`;

			const rawJudgeFollowers = await db.sequelize.query(judgeFollowersQuery, {
				replacements,
				type: db.sequelize.QueryTypes.SELECT,
			});

			rawJudgeFollowers.forEach( (person) => {
				if (person.provider && person.phone) {
					blastMe.text.push(`${person.phone}@${person.provider}`);
				}
				if (person.email) {
					blastMe.email.push(`${person.email}`);
				}
			});
		}
	}

	// Deduplicate the emails
	blastMe.text = Array.from(new Set(blastMe.text));
	blastMe.email = Array.from(new Set(blastMe.email));

	return blastMe;

};

export default followers;
