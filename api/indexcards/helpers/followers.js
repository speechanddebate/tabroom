import db from './db';

export const getFollowers = async (replacements, options = { recipients: 'all' }) => {

	let whereLimit = '';
	let fields = '';

	if (replacements.panelId) {
		whereLimit = ` where panel.id = :panelId `;
		delete replacements.roundId;
	} else if (replacements.sectionId) {
		whereLimit = ` where panel.id = :sectionId `;
		delete replacements.roundId;
	} else if (replacements.roundId) {
		whereLimit = ` where panel.round = :roundId `;
	} else if (replacements.timeslotId) {
		whereLimit = ` where panel.round = round.id and round.timeslot = :timeslotId `;
		fields = ',round';
	} else {
		return { error: true, message: `No round or section to blast sent` };
	}

	if (options.flight) {
		whereLimit +=  ` and panel.flight = :panelFlight `;
		replacements.panelFlight = options.flight;
	}

	const blastMe = {
		phone : [],
		email : [],
		error : false,
	};

	const blastOnly = {
		entry  : {},
		judge  : {},
		school : {},
	};

	if (options.recipients !== 'judges') {

		let entryPeopleQuery = `
			select
				person.id, person.email, person.phone, person.provider, entry.id entry, entry.school school
			from (panel, person, ballot, entry, entry_student es, student ${fields})
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

		if (options.status === 'unstarted' ) {
			entryPeopleQuery += `
				and (ballot.judge_started IS NULL)
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		} else if (options.status === 'unentered' ) {
			entryPeopleQuery += `
				and NOT EXISTS (
					select score.id from score where score.ballot = ballot.id
				)
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		} else if (options.status === 'unconfirmed' ) {
			entryPeopleQuery += `
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		}

		const rawEntryPeople = await db.sequelize.query(entryPeopleQuery, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawEntryPeople.forEach( (person) => {
			if (person.provider && person.phone) {
				blastMe.phone.push(`${person.phone}@${person.provider}`);

				if (person.entry) {
					if (!blastOnly.entry[person.entry]) {
						blastOnly.entry[person.entry] = {
							phone : [],
							email : [],
						};
					}
					blastOnly.entry[person.entry].phone.push(`${person.phone}@${person.provider}`);
				}
			}

			if (person.email) {
				blastMe.email.push(person.email);

				if (person.entry) {
					if (!blastOnly.entry[person.entry]) {
						blastOnly.entry[person.entry] = {
							phone : [],
							email : [],
						};
					}
					blastOnly.entry[person.entry].email.push(person.email);
				}
			}
		});
	}

	if (options.recipients !== 'entries') {

		let judgePeopleQuery = `
			select
				person.id, person.email, person.phone, person.provider, judge.id judge, judge.school school
			from (person, ballot, judge, panel ${fields})
			${whereLimit}
				and ballot.panel = panel.id
				and ballot.judge = judge.id
				and judge.person = person.id
				and person.no_email = 0
		`;

		if (options.status === 'unstarted' ) {
			judgePeopleQuery += `
				and (ballot.judge_started IS NULL)
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		} else if (options.status === 'unentered' ) {
			judgePeopleQuery += `
				and NOT EXISTS (
					select score.id from score where score.ballot = ballot.id
				)
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		} else if (options.status === 'unconfirmed' ) {
			judgePeopleQuery += `
				and (ballot.audit = 0 OR ballot.audit IS NULL)
			`;
		}

		const rawJudgePeople = await db.sequelize.query(judgePeopleQuery, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawJudgePeople.forEach( (person) => {
			if (person.provider && person.phone) {
				blastMe.phone.push(`${person.phone}@${person.provider}`);

				if (person.judge) {
					if (!blastOnly.judge[person.judge]) {
						blastOnly.judge[person.judge] = {
							phone : [],
							email : [],
							self  : [],
						};
					}
					blastOnly.judge[person.judge].phone.push(`${person.phone}@${person.provider}`);
				}
			}

			if (person.email) {

				blastMe.email.push(person.email);

				if (person.judge) {
					if (!blastOnly.judge[person.judge]) {
						blastOnly.judge[person.judge] = {
							phone : [],
							email : [],
							self  : [],
						};
					}
					blastOnly.judge[person.judge].self.push(person.email);
				}
			}
		});
	}

	if (!options.no_followers) {

		if (options.recipients !== 'judges') {

			const entryFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider, entry.id entry
				from (person, ballot, entry, follower, panel ${fields})
				${whereLimit}
					and ballot.panel = panel.id
					and ballot.entry = entry.id
					and entry.active = 1
					and entry.id = follower.entry
					and follower.person = person.id
					and person.no_email = 0
				group by person.id
			`;

			const rawEntryFollowers = await db.sequelize.query(entryFollowersQuery, {
				replacements,
				type: db.sequelize.QueryTypes.SELECT,
			});

			rawEntryFollowers.forEach( (person) => {

				if (person.provider && person.phone) {
					blastMe.phone.push(`${person.phone}@${person.provider}`);
				}

				if (person.email) {
					blastMe.email.push(person.email);
				}

				if (person.entry) {
					if (!blastOnly.entry[person.entry]) {
						blastOnly.entry[person.entry] = {
							phone : [],
							email : [],
						};
					}
					if (person.phone && person.provider) {
						blastOnly.entry[person.entry].phone.push(`${person.phone}@${person.provider}`);
					}
					blastOnly.entry[person.entry].email.push(person.email);
				}
			});

			const schoolFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider, follower.school school
				from (person, ballot, entry, follower, panel ${fields})
				${whereLimit}
					and ballot.panel = panel.id
					and ballot.entry = entry.id
					and entry.active = 1
					and entry.school = follower.school
					and follower.person = person.id
					and person.no_email = 0
			`;

			const rawSchoolFollowers = await db.sequelize.query(schoolFollowersQuery, {
				replacements,
				type: db.sequelize.QueryTypes.SELECT,
			});

			rawSchoolFollowers.forEach( (person) => {
				if (person.email) {
					if (!blastOnly.school[person.school]) {
						blastOnly.school[person.school] = {
							email : [],
						};
					}
					blastOnly.school[person.school].email.push(person.email);
				}
			});
		}

		if (options.recipients !== 'entries') {

			const judgeFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider, ballot.judge judge
				from (person, ballot, follower, panel ${fields})
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
					blastMe.phone.push(`${person.phone}@${person.provider}`);
				}
				if (person.email) {
					blastMe.email.push(person.email);
				}

				if (person.judge) {
					if (!blastOnly.judge[person.judge]) {
						blastOnly.judge[person.judge] = {
							phone : [],
							email : [],
						};
					}

					if (person.phone && person.provider) {
						blastOnly.judge[person.judge].phone.push(`${person.phone}@${person.provider}`);
					}
					blastOnly.judge[person.judge].email.push(`${person.email}`);
				}
			});

			const schoolFollowersQuery = `
				select
					person.id, person.email, person.phone, person.provider, ballot.judge judge
				from (person, judge, ballot, follower, panel ${fields})
				${whereLimit}
					and ballot.panel = panel.id
					and ballot.judge = judge.id
					and judge.school = follower.school
					and judge.school > 0
					and follower.person = person.id
					and person.no_email = 0
			`;

			const rawSchoolFollowers = await db.sequelize.query(schoolFollowersQuery, {
				replacements,
				type: db.sequelize.QueryTypes.SELECT,
			});

			rawSchoolFollowers.forEach( (person) => {
				if (person.email) {
					if (!blastOnly.school[person.school]) {
						blastOnly.school[person.school] = {
							email : [],
						};
					}
					blastOnly.school[person.school].email.push(person.email);
				}
			});
		}
	}

	// Deduplicate the emails
	blastMe.phone = Array.from(new Set(blastMe.phone));
	blastMe.email = Array.from(new Set(blastMe.email));
	blastMe.only  = { ...blastOnly };
	blastMe.recipients = options.recipients;

	return blastMe;

};

export default getFollowers;
