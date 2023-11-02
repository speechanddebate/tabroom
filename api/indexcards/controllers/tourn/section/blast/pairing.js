/* eslint-disable no-restricted-syntax */
import moment from 'moment-timezone';
import { ordinalize } from '@speechanddebate/nsda-js-utils';
import { getPairingFollowers } from '../../../../helpers/followers';
import { notify } from '../../../../helpers/pushNotify';
import { sectionCheck, timeslotCheck, roundCheck } from '../../../../helpers/auth.js';
import wudcPosition from '../../../../helpers/textmunge';
import { sidelocks } from '../../../../helpers/round';
import scheduleAutoFlip from './autoFlip';
import config from '../../../../../config/config';

// Blast a single section with a pairing
export const blastSection = {
	POST: async (req, res) => {

		// Permissions.  I feel like there should be a better way to do this
		const permOK = await sectionCheck(req, res, req.params.sectionId);
		if (!permOK) { return; }

		const queryData = {};
		queryData.replacements = { sectionId : req.params.sectionId };
		queryData.where = 'where section.id = :sectionId';
		queryData.fields = '';

		const blastData = await formatBlast(queryData, req);

		const followers = await getPairingFollowers(
			queryData.replacements,
			{ ...req.body },
		);

		if (req.body.append) {
			blastData.append = req.body.append;
		}

		sendBlast(followers, blastData, req, res);
	},
};

// Blast a single round with a pairing
export const blastRound = {
	POST: async (req, res) => {

		let sender = 0;

		if (req.body.share_key === config.SHARE_KEY) {

			sender = req.body.sender;

		} else {
			const permOK = await roundCheck(req, res, req.params.roundId);
			if (!permOK) {
				return;
			}

			sender = req.session.person;
		}

		const queryData = {};
		queryData.replacements = { roundId : req.params.roundId };
		queryData.where = 'where section.round = :roundId';
		queryData.fields = '';

		if (req.body.publish) {
			await req.db.sequelize.query(
				`update round set published = 1 where round.id = :roundId `, {
					replacements : queryData.replacements,
					type         : req.db.sequelize.QueryTypes.UPDATE,
				});

			await req.db.changeLog.create({
				tag         : 'publish',
				description : `Round published`,
				person      : sender,
				round       : req.params.roundId,
			});
			await scheduleAutoFlip(req.params.roundId, req, res);
		}

		await req.db.sequelize.query(
			`delete from round_setting where round = :roundId and tag = 'blasted'`, {
				replacements : queryData.replacements,
				type         : req.db.sequelize.QueryTypes.UPDATE,
			});

		await req.db.sequelize.query(
			`insert into round_setting (tag, round, value_date, value) values ('blasted', :roundId, now(), 'date')`, {
				replacements : queryData.replacements,
				type         : req.db.sequelize.QueryTypes.UPDATE,
			});

		const blastData = await formatBlast(queryData, req);
		const followers = await getPairingFollowers(
			queryData.replacements,
			{ ...req.body },
		);

		if (req.body.message) {
			blastData.append = req.body.message;
		}
		if (req.body.append) {
			blastData.append = req.body.append;
		}

		sendBlast(followers, blastData, req, res);
	},
};

// Blast a whole timeslot's rounds with pairings
export const blastTimeslot = {
	POST: async (req, res) => {

		const permOK = await timeslotCheck(req, res, req.params.timeslotId);
		if (!permOK) {
			return;
		}

		const queryData = {};
		queryData.replacements = { timeslotId : req.params.timeslotId };
		queryData.fields = ', round';
		queryData.where = 'where round.timeslot = :timeslotId and round.id = section.round ';

		const tsRounds = await req.db.sequelize.query(
			`select round.id, round.name, round.type from round where round.timeslot = :timeslotId`,
			{
				replacements : queryData.replacements,
				type         : req.db.sequelize.QueryTypes.SELECT,
			});

		await req.db.sequelize.query(
			`delete rs.* from round_setting rs, round where round.timeslot = :timeslotId and round.id = rs.round and rs.tag = 'blasted'`,
			{
				replacements : queryData.replacements,
				type         : req.db.sequelize.QueryTypes.DELETE,
			});

		tsRounds.forEach( async (row) => {
			await req.db.sequelize.query(
				`insert into round_setting (tag, round, value_date, value) values ('blasted', :roundId, now(), 'date')`,
				{
					replacements : { roundId: row.id },
					type         : req.db.sequelize.QueryTypes.UPDATE,
				});
		});

		if (req.body.publish) {
			await req.db.sequelize.query(
				`update round set published = 1 where round.timeslot = :timeslotId `,
				{
					replacements : queryData.replacements,
					type         : req.db.sequelize.QueryTypes.UPDATE,
				});

			tsRounds.forEach( async (row) => {
				await scheduleAutoFlip(row.id, req, res);
				await req.db.changeLog.create({
					tag         : 'publish',
					description : `Round published`,
					person      : req.session.person,
					round       : row.id,
				});
			});
		}

		const blastData = await formatBlast(queryData, req);

		const followers = await getPairingFollowers(
			queryData.replacements,
			{ ...req.body },
		);
		sendBlast(followers, blastData, req, res);
	},
};

// formatBlast pulls the sql paramters from the blastSection/Round/Timeslot
// calling function and then

const formatBlast = async (queryData, req) => {

	const entryQuery = `
		select
			section.id sectionid, section.round roundid,
			entry.id, entry.code, entry.name,
			entry.school schoolid, school.name schoolname,
			ballot.id ballot, ballot.side, ballot.speakerorder,
			GROUP_CONCAT(DISTINCT(CONCAT(student.first,' ',': ',person.pronoun)) SEPARATOR ' ') pronoun
		from (panel section, ballot, entry, entry_student es, student ${queryData.fields})
			left join person on student.person = person.id and person.pronoun IS NOT NULL and person.pronoun != ''
			left join school on entry.school = school.id
		${queryData.where}
			and section.id = ballot.panel
			and ballot.entry = entry.id
			and entry.id = es.entry
			and es.student = student.id
		group by entry.id
		order by section.flight, section.id, ballot.side, ballot.speakerorder
	`;

	const judgeQuery = `
		select
			section.id sectionid, section.round roundid,
			judge.id, judge.first, judge.middle, judge.last, judge.code,
			school.id schoolid, school.name schoolname,
			ballot.chair, person.pronoun
		from (ballot, judge, panel section ${queryData.fields})
			left join person on judge.person = person.id
			left join school on judge.school = school.id
		${queryData.where}
			and section.id = ballot.panel
			and ballot.judge = judge.id
		group by judge.id, section.id
		order by section.flight, section.letter, ballot.chair DESC, judge.last
	`;

	const roundQuery = `
		select
			section.id sectionid, section.letter, section.bye sbye, section.flight,
			round.id roundid, round.name roundname, round.label roundlabel,
			round.type roundtype, round.flighted roundflights,
			room.id roomid, room.name roomname, room.url roomurl, room.notes roomnotes,
			round.start_time roundstart,
			event.id eventid, event.abbr eventabbr, event.name eventname,
			event.type eventtype,
			include_room_notes.value include_room_notes,
			use_normal_rooms.value use_normal_rooms,
			event_online_hybrid.value event_online_hybrid,
			online_hybrid.value online_hybrid,
			online_mode.value online_mode,
			anonymous_public.value anonymous_public,
			sidelock_elims.value sidelock_elims,
			no_side_constraints.value no_side_constraints,
			flight_offset.value flight_offset,
			flip_at.value_date flip_at,
			aff_label.value aff_label,
			neg_label.value neg_label,
			tourn.tz

		from (tourn, event, round, panel section)

			left join room on room.id = section.room

			left join round_setting include_room_notes
				on include_room_notes.round = round.id
				and include_room_notes.tag = 'include_room_notes'

			left join round_setting use_normal_rooms
				on use_normal_rooms.round = round.id
				and use_normal_rooms.tag = 'use_normal_rooms'

			left join event_setting aff_label
				on aff_label.event = event.id
				and aff_label.tag = 'aff_label'

			left join event_setting neg_label
				on neg_label.event = event.id
				and neg_label.tag = 'neg_label'

			left join event_setting anonymous_public
				on anonymous_public.event = event.id
				and anonymous_public.tag = 'anonymous_public'

			left join event_setting online_mode
				on online_mode.event = event.id
				and online_mode.tag = 'online_mode'

			left join event_setting no_side_constraints
				on no_side_constraints.event = event.id
				and no_side_constraints.tag = 'no_side_constraints'

			left join event_setting sidelock_elims
				on sidelock_elims.event = event.id
				and sidelock_elims.tag = 'sidelock_elims'

			left join event_setting flight_offset
				on flight_offset.event = event.id
				and flight_offset.tag = 'flight_offset'

			left join event_setting event_online_hybrid
				on event_online_hybrid.event = event.id
				and event_online_hybrid.tag = 'online_hybrid'

			left join panel_setting online_hybrid
				on online_hybrid.panel = section.id
				and online_hybrid.tag = 'online_hybrid'

			left join panel_setting flip_at
				on flip_at.panel = section.id
				and flip_at.tag = 'flip_at'

		${queryData.where}
			and section.round = round.id
			and round.event = event.id
			and event.tourn = tourn.id
		group by section.id
			order by round.name, section.flight, section.letter
	`;

	const rawRoundData = await req.db.sequelize.query(roundQuery, {
		replacements : queryData.replacements,
		type         : req.db.sequelize.QueryTypes.SELECT,
	});

	const rawEntries = await req.db.sequelize.query(entryQuery, {
		replacements : queryData.replacements,
		type         : req.db.sequelize.QueryTypes.SELECT,
	});

	const rawJudges = await req.db.sequelize.query(judgeQuery, {
		replacements : queryData.replacements,
		type         : req.db.sequelize.QueryTypes.SELECT,
	});

	const roundData = await processRounds(rawRoundData);

	rawEntries.forEach( (row) => {
		const section = roundData[row.roundid].sections[row.sectionid];
		section.entries.push({
			id         : row.id,
			code       : row.code,
			name       : row.name,
			school     : row.schoolid,
			schoolName : row.schoolname,
			side       : row.side,
			speaker    : row.speakerorder,
			pronoun    : row.pronoun,
		});
	});

	rawJudges.forEach( (row) => {
		const section = roundData[row.roundid].sections[row.sectionid];
		section.judges.push({
			id         : row.id,
			code       : row.code,
			first      : row.first,
			middle     : row.middle,
			last       : row.last,
			school     : row.schoolid,
			schoolName : row.schoolname,
			chair      : row.chair,
			pronoun    : row.pronoun,
		});
	});

	// Now format round and section messages for each recipient.

	// I am vaguely aware that at this point most JS writers would shove the
	// thing off into another function, so I thought about it, but I couldn't
	// come up with a good reason why. It's not like one long function is
	// harder to parse than a logical flow with bits spread around the file at
	// random.

	const blastData = {
		entries       : {},
		judges        : {},
		schools       : {},
		rounds        : [],
		schoolEntries : {},
		schoolJudges  : {},
	};

	Object.keys(roundData).forEach( (roundId) => {

		const round = roundData[roundId];
		const roundMessage = { };
		roundMessage.text = `${round.name} of ${round.eventAbbr}\n`;
		roundMessage.html = `<p style='font-weight: 600; width: 50%; display: inline-block;'>${round.name} of ${round.eventName}</p>`;

		const sectionFlights = Object.keys(round.flightSections).sort( (a, b) => {
			return a - b;
		});

		let counter = 1;

		sectionFlights.forEach( (flight) => {
			Object.keys(round.flightSections[flight]).forEach( (sectionId) => {
				const section = round.sections[sectionId];

				const sectionMessage = {
					...roundMessage,
					single: '',
					judgeEntrySingle: '',
					entrySingle: '',
				};

				if (round.flights > 1) {
					sectionMessage.text += `\nFlight ${section.flight} \n`;
					sectionMessage.html += `<p style="width: 25%; display: inline-block;">Flight ${section.flight}</p>`;
					sectionMessage.single += `\n\tFlt ${section.flight} Start ${round.shortstart[section.flight]}\n`;
				}

				sectionMessage.text += `Start ${round.shortstart[section.flight]} \n`;
				sectionMessage.text += `Room: ${section.room} `;

				sectionMessage.html += `<p>Start ${round.start[section.flight]}</p>`;
				sectionMessage.html += `<p>Room: ${section.room} `;
				sectionMessage.single += `\tRoom ${section.room} Counter ${counter++} Letter ${section.letter}`;

				if (section.hybrid) {
					sectionMessage.text += ` (OL/HYB) \n`;
					sectionMessage.html += ` (ONLINE HYBRID)</p>`;
					sectionMessage.single += ` (HYB) `;
				} else {
					sectionMessage.text += `\n`;
					sectionMessage.single += `\n`;
					sectionMessage.html += `</p>`;
				}
				if (section.map) {
					sectionMessage.text += `Map Link on Tabroom \n`;
					sectionMessage.html += `<p style='width: 75%; display: inline-block; text-align: center;'><a style="font-size: 90%;" href="${section.map}" alt="Map Link">Map to ${section.room}</a></p>`;
				}
				if (section.url) {
					sectionMessage.text += `Video Link on Tabroom \n`;
					sectionMessage.html += `<p style='width: 75%; display: inline-block; text-align: center;'><a style="font-size: 90%;" href="${section.url}" alt="Video Link">Video Link for ${section.room}</a></p>`;
				}

				// Create standard texts for lists of entries & judges for the other to see
				if (!round.eventType !== 'mock_trial') {
					sectionMessage.judgeText = `\nJudging\n`;
					sectionMessage.judgeSingle = `\n\tJudges:`;
					sectionMessage.judgeHTML = `<p style="font-weight: 600;">Judging</p>`;

					let firstJudge = 0;

					section.judges.forEach( (judge) => {

						judge.role = judgeRole(judge, round) || '';

						if (round.settings.anonymous_public) {
							sectionMessage.judgeText += `${judge.role} ${judge.code} `;
							sectionMessage.judgeSingle += `${judge.role} ${judge.code} `;
							sectionMessage.judgeHTML += `<p> ${judge.role} ${judge.code} `;
						} else {
							sectionMessage.judgeText += `${judge.role} ${judge.first} ${judge.last}`;
							if (firstJudge++ > 0) {
								sectionMessage.judgeSingle += ', ';
							}
							sectionMessage.judgeSingle += `${judge.role} ${judge.first} ${judge.last}`;
							sectionMessage.judgeHTML += `<p> ${judge.role} ${judge.first} ${judge.middle ? `${judge.middle} ` : ''}${judge.last}`;
							if (judge.pronoun) {
								sectionMessage.judgeText += ` (${judge.pronoun})`;
								sectionMessage.judgeHTML += `<p style='font-style: italic; font-size: 90%; padding-left: 8pt;'>${judge.pronoun}</p>`;
							}
						}
						sectionMessage.judgeText += `\n`;
						sectionMessage.judgeHTML += `</p>`;
					});
				}

				sectionMessage.entryText = `\nEntries\n`;
				sectionMessage.entryHTML = `<p style="font-weight: 600;">Competitors</p>`;

				// I guess they don't necessarily want to have the recency out
				// there for some reason.

				if (round.eventType === 'congress') {
					section.entries.sort((a, b) => {
						if (a.name < b.name) {
							return -1;
						}
						if (a.name > b.name) {
							return 1;
						}
						return 0;
					});
				}

				let notFirstEntry = 0;

				section.entries.forEach( (entry) => {
					entry.position = positionString(entry, round, section);

					if (entry.position === 'FLIP' && notFirstEntry++ < 1) {
						sectionMessage.entryHTML += `<p>FLIP FOR SIDES:</p>`;
						sectionMessage.entryText += `FLIP FOR SIDES:\n`;
					}

					sectionMessage.entryText += `${entry.position === 'FLIP' ? '' : entry.position} ${entry.code} `;
					sectionMessage.entryHTML += `<p>${entry.position === 'FLIP' ? '' : entry.position} ${entry.code} `;

					if (entry.pronoun && !round.settings.anonymous_public) {
						sectionMessage.entryText += `(${entry.pronoun})`;
						sectionMessage.entryHTML += `<p style='font-style: italic; font-size: 90%; padding-left: 8pt;'>${entry.pronoun}</p>`;
					}
					sectionMessage.entryText += `\n`;
					sectionMessage.entryHTML += `</p>`;

					if (round.eventType === 'congress') {
						delete sectionMessage.entryText;
					} else if (round.eventType === 'debate' || round.eventType === 'wsdc') {
						section.entries.forEach( (other) => {
							if (entry.id !== other.id) {
								entry.opponent = other.code;
							}
						});
						if (sectionMessage.judgeEntrySingle) {
							sectionMessage.judgeEntrySingle += ' vs. ';
							sectionMessage.judgeEntrySingle += `${entry.position === 'FLIP' ? '' : entry.position} ${entry.code} `;
						} else {
							sectionMessage.judgeEntrySingle += `${entry.position} ${entry.code} `;
						}
					} else if (round.eventType === 'wudc') {
						sectionMessage.entrySingle += `${entry.position} ${entry.code} `;
					}
				});

				// And now that we have the standard texts, we can assemble the
				// notifications for the actual entries

				section.entries.forEach( (entry) => {

					// Myself
					if (!blastData.entries[entry.id]) {
						blastData.entries[entry.id] = {
							subject: `${entry.code} ${round.name} ${round.eventAbbr}`,
							text : sectionMessage.text,
							html : sectionMessage.html,
						};
					}

					const entryMessage = blastData.entries[entry.id];

					if (entry.position === 'FLIP') {
						entryMessage.text += 'Flip for Sides\n';
						entryMessage.flip += '<p>Flip for Sides</p>';
					} else if (entry.position && round.eventType === 'speech') {
						entryMessage.text += `Speak ${entry.position} \n`;
						entryMessage.html += `<p>You will speak ${entry.position}</p>`;
					} else if (entry.position) {
						entryMessage.text += `Side ${entry.position} \n`;
						entryMessage.html += `<p>Side: ${entry.position}</p>`;
					}

					if (round.eventType !== 'congress') {
						entryMessage.text += sectionMessage.entryText;
					}

					entryMessage.html += sectionMessage.entryHTML;

					if (round.eventType !== 'mock_trial') {
						entryMessage.text += sectionMessage.judgeText;
						entryMessage.html += sectionMessage.judgeHTML;
					}

					// My school
					if (entry.school && blastData.schoolEntries ) {
						if (!blastData.schools[entry.school]) {
							blastData.schools[entry.school] = {
								subject : `${entry.schoolName} Round Assignments `,
								text    : `Full assignments for ${entry.schoolName}\n`,
							};
						}

						if (!blastData.schoolEntries[entry.school]) {
							blastData.schoolEntries[entry.school] = {
								text : '',
								done : {},
							};
						}
						const schoolMessage = blastData.schoolEntries[entry.school];

						if (!schoolMessage.done[round.id]) {
							schoolMessage.done[round.id] = true;
						}

						schoolMessage.text += `${entry.code} `;

						if (entry.opponent) {
							schoolMessage.text += `\n\t ${entry.position} vs ${entry.opponent} `;
						} else if (sectionMessage.entrySingle) {
							schoolMessage.text += sectionMessage.entrySingle;
						} else if (round.eventType === 'speech') {
							schoolMessage.text += ` Speaks ${entry.position} `;
						} else if (entry.position) {
							schoolMessage.text += ` ${entry.position} `;
						}

						if (round.eventType !== 'mock_trial') {
							schoolMessage.text += sectionMessage.judgeSingle;
						}

						schoolMessage.text +=  sectionMessage.single;
						schoolMessage.text += '\n';
					}
				});

				section.judges.forEach( (judge) => {
					// Myself
					if (!blastData.judges[judge.id]) {
						blastData.judges[judge.id] = {
							subject: `${judge.first} ${judge.last} ${round.eventAbbr} ${round.name}`,
						};
					}

					const judgeMessage = blastData.judges[judge.id];

					if (!judgeMessage.text) {
						judgeMessage.text = '';
						judgeMessage.html = '';
					}

					judgeMessage.text += sectionMessage.text;
					judgeMessage.html += sectionMessage.html;

					judge.role = judgeRole(judge, round);
					if (judge.role) {
						judgeMessage.text += `Role: ${judge.role}\n`;
						judgeMessage.html += `<p>Role: ${judge.role}</p>`;
					}

					if (round.eventType === 'mock_trial') {
						let firstJudge = 0;
						section.judges.forEach( (other) => {

							if (firstJudge++ > 0) {
								judgeMessage.text += ', ';
							}

							other.role = `${judgeRole(other, round)} `;
							judgeMessage.text += `${other.role}${other.first} ${other.last} `;
							judgeMessage.html += `<p> ${other.role}${other.first} ${other.middle ? `${other.middle} ` : ''}${other.last} `;

							if (other.pronoun && !round.settings.anonymous_public) {
								judgeMessage.text += `(${judge.pronoun})`;
								judgeMessage.html += `<p style='font-style: italic; font-size: 90%; padding-left: 8pt;'>${judge.pronoun}</p>`;
							}
						});

						judgeMessage.text += `\n`;
						judgeMessage.html += `</p>`;
					}

					judgeMessage.text += sectionMessage.entryText;
					judgeMessage.html += sectionMessage.entryHTML;

					if (sectionMessage.judgeText) {
						judgeMessage.text += sectionMessage.judgeText;
						judgeMessage.html += sectionMessage.judgeHTML;
					}

					// My school
					if (judge.school && blastData.schoolJudges) {
						if (!blastData.schools[judge.school]) {
							blastData.schools[judge.school] = {
								subject : `${judge.schoolName} Round Assignments `,
								text    : `Full assignments for ${judge.schoolName}\n`,
							};
						}
						if (!blastData.schoolJudges[judge.school]) {
							blastData.schoolJudges[judge.school] = {
								text : '',
								done : {},
							};
						}

						const schoolMessage = blastData.schoolJudges[judge.school];

						if (!schoolMessage.done[round.id]) {
							schoolMessage.done[round.id] = true;
						}

						schoolMessage.text += `${judge.code ? `${judge.code} ` : ''}${judge.first} ${judge.last} `;
						schoolMessage.text += sectionMessage.judgeEntrySingle ? sectionMessage.judgeEntrySingle : '';
						schoolMessage.text += sectionMessage.entrySingle ? sectionMessage.entrySingle : '';
						schoolMessage.text += sectionMessage.judgeSingle ? sectionMessage.judgeSingle : '';
						schoolMessage.text += `${sectionMessage.single} `;
						schoolMessage.text += '\n';
					}
				});
			});
		});

		Object.keys(blastData.schools).forEach( (schoolId) => {
			blastData.schools[schoolId].text += `\n${round.eventAbbr} ${round.name} Start ${round.shortstart[1]}\n\n`;
			if (blastData.schoolEntries?.[schoolId]) {
				blastData.schools[schoolId].text += `ENTRIES\n${blastData.schoolEntries[schoolId].text}`;
			}
			if (blastData.schoolJudges?.[schoolId]) {
				blastData.schools[schoolId].text += `JUDGES\n${blastData.schoolJudges[schoolId].text}`;
			}
		});

		delete blastData.schoolJudges;
		delete blastData.schoolEntries;
		blastData.rounds.push(round);
	});

	return blastData;
};

// Create the actual transporter maps for each blast email and text. Note that
// this is keyed to the followers.only data structure because that tells us who
// is following an individual entry or judge, not just a follower of someone in
// the round as a whole

const sendBlast = async (followers, blastData, req, res) => {

	const blastResponse = {
		error   : false,
		email   : 0,
		web     : 0,
		message : '',
	};

	for await (const entryId of Object.keys(blastData.entries)) {
		if (followers.entries[entryId]) {
			const notifyResponse = await notify({
				ids    : followers.entries[entryId],
				append : blastData.append,
				...blastData.entries[entryId],
			});

			blastResponse.email += notifyResponse.email.count;
			blastResponse.web += notifyResponse.web.count;

			if (notifyResponse.error) {
				blastResponse.error = true;
				blastResponse.message += notifyResponse.message;
			}
		}
	}

	for await (const judgeId of Object.keys(blastData.judges)) {
		if (followers.judges[judgeId]) {
			const notifyResponse = await notify({
				ids    : followers.judges[judgeId],
				append : blastData.append,
				...blastData.judges[judgeId],
			});

			blastResponse.email += notifyResponse.email.count;
			blastResponse.web += notifyResponse.web.count;

			if (notifyResponse.error) {
				blastResponse.error = true;
				blastResponse.message += notifyResponse.message;
			}
		}
	}

	for await (const schoolId of Object.keys(blastData.schools)) {

		if (followers.schools[schoolId]) {
			const notifyResponse = await notify({
				ids    : followers.schools[schoolId],
				append : blastData.append,
				...blastData.schools[schoolId],
			});

			blastResponse.email += notifyResponse.email.count;
			blastResponse.web += notifyResponse.web.count;

			if (notifyResponse.error) {
				blastResponse.error = true;
				blastResponse.message += notifyResponse.message;
			}
		}
	}

	if (blastResponse.error) {

		res.status(200).json({
			blastError   : blastResponse.error,
			blastMessage : blastResponse.message,
			emailCount   : blastResponse.email,
			webCount     : blastResponse.web,
		});

	} else {

		if (req.params.sectionId) {

			await req.db.changeLog.create({
				tag         : 'blast',
				description : `Pairing sent to section. Message: ${req.body.message}`,
				person      : blastData.sender || req.session?.person,
				count       : blastResponse.web,
				panel       : req.params.sectionId,
			});

			await req.db.changeLog.create({
				tag         : 'emails',
				description : `Pairing sent to section. Message: ${req.body.message}`,
				person      : blastData.sender || req.session?.person,
				count       : blastResponse.email,
				panel       : req.params.sectionId,
			});

		} else {

			blastData.rounds.forEach( async (round) => {

				await req.db.changeLog.create({
					tag         : 'blast',
					description : `Pairing sent. Message: ${req.body.message}`,
					person      : blastData.sender || req.session?.person,
					count       : blastResponse.web,
					round       : round.id,
				});

				await req.db.changeLog.create({
					tag         : 'emails',
					description : `Pairing sent. Message: ${req.body.message}`,
					person      : blastData.sender || req.session?.person,
					count       : blastResponse.email,
					round       : round.id,
				});
			});
		}

		const browserResponse = {
			error   : false,
			message : `Pairings sent to ${blastResponse.web} web and ${blastResponse.email} email recipients`,
		};

		res.status(200).json(browserResponse);
	}
};

const processRounds = async (rawRounds) => {

	const roundData  = { };

	rawRounds.forEach( (row) => {

		if (!roundData[row.roundid]) {

			const round = {
				id             : row.roundid,
				name           : row.roundlabel ? row.roundlabel : `Round ${row.roundname}`,
				number         : row.roundname,
				type           : row.roundtype,
				flights        : row.roundflights,
				eventId        : row.eventid,
				eventName      : row.eventname,
				eventAbbr      : row.eventabbr,
				eventType      : row.eventtype,
				flip           : false,
				start          : {},
				shortstart     : {},
				flipAt         : {},
				settings       : {},
				sections       : {},
				flightSections : {},
				schools        : {},
				sidelocks      : {},
			};

			['include_room_notes',
				'use_normal_rooms',
				'anonymous_public',
				'online_mode',
				'event_online_hybrid',
				'aff_label',
				'neg_label',
			].forEach( (key) => {
				if (row[key]) {
					round.settings[key] = row[key];
				}
			});

			if (round.eventType === 'debate' || round.eventType === 'wsdc') {
				if (row.sidelock_elims) {
					round.flip = false;
				} else if (row.no_side_constraints) {
					round.flip = true;
				} else if (round.type === 'elim' || round.type === 'final' || round.type === 'runoff') {
					const locks = sidelocks(round.id);
					round.sidelocks = { ...locks };
					round.flip = true;
				}
			}

			// Every time I look for "the easy way to do something" that
			// was like 4 characters in Perl the answer ends up being
			// something like this monstrosity. 'Twas foreach (1 ... n) {}.
			// Progress! Sigh.

			[...Array(round.flights).keys()].forEach( (tick) => {
				const flight = tick + 1;
				round.start[flight] = '';
				round.shortstart[flight] = '';

				round.start[flight] =
					moment(row.roundstart)
						.add(parseInt(tick * row.flight_offset), 'minutes')
						.tz(row.tz)
						.format('h:mm z');

				round.shortstart[flight] =
					moment(row.roundstart)
						.add(parseInt(tick * row.flight_offset), 'minutes')
						.tz(row.tz)
						.format('h:mm');

				if (round.flip && row.flip_split_flights && row.flip_at) {
					round.flipAt[flight] =
						moment(row.flip_at)
							.add(parseInt(tick * row.flight_offset), 'minutes')
							.tz(row.tz)
							.format('h:mm');
				} else if (round.flip && row.flip_at) {
					round.flipAt[flight] =
						moment(row.flip_at)
							.tz(row.tz)
							.format('h:mm');
				}
			});

			roundData[row.roundid] = round;
		}

		const round = roundData[row.roundid];

		// Now append the section data

		const section = {
			id      : row.sectionid,
			letter  : row.letter,
			flight  : row.flight,
			bye     : row.sbye,
			start   : round.start[row.flight ? row.flight : 1],
			online  : false,
			room    : '',
			map     : '',
			url     : '',
			entries : [],
			judges  : [],
		};

		if (row.sbye) {
			section.room = 'BYE';
		} else if (!round.settings.online_mode || round.settings.online_mode === 'none') {
			section.room = row.roomname ? row.roomname : 'NONE : ASK TAB';
			section.map  = row.roomurl;
		} else {

			if (round.settings.online_mode === 'async') {
				section.room = 'ASYNC';

			} else if (round.settings.event_online_hybrid) {
				section.hybrid = row.online_hybrid;
				section.room = row.roomname ? row.roomname : 'NONE';
				section.url  = row.roomurl;

			} else if (round.settings.online_mode === 'sync' || round.settings.use_normal_rooms) {
				section.room   = row.roomname ? row.roomname : 'NONE: ASK TAB';
				section.url    = row.roomurl;

			} else if (round.settings.online_mode === 'nsda_campus'
				|| round.settings.online_mode === 'nsda_campus_observers'
				|| round.settings.online_mode === 'nsda_private'
				|| round.settings.online_mode === 'public_jitsi'
				|| round.settings.online_mode === 'public_jitsi_observers'
			) {
				section.room   = `NSDA Campus Section ${row.letter}`;
			} else {
				section.room = row.roomname ? row.roomname : 'ONLINE SETTINGS ARE REALLY GOOFY. ASK TAB';
			}
		}

		if (round.settings.include_room_notes && row.roomnotes) {
			section.roomnotes = row.roomnotes;
		}

		if (!round.flightSections[row.flight]) {
			round.flightSections[row.flight] = {};
		}

		round.flightSections[row.flight][row.sectionid] = section;
		round.sections[row.sectionid] = section;

	});

	return roundData;
};

const judgeRole = (judge, round) => {

	if (judge.chair) {
		if (round.eventType === 'congress') {
			return 'Parliamentarian';
		}
		if (round.eventType === 'mock_trial') {
			return 'Presiding Judge';
		}
		return 'Chair';
	}

	if (round.eventType === 'congress' || round.eventType === 'mock_trial') {
		return 'Scorer';
	}

	if (round.eventType === 'wudc') {
		return 'Wing';
	}
};

const positionString = (entry, round, section) => {

	if (
		round.eventType === 'mock_trial'
		|| round.eventType === 'wsdc'
		|| round.eventType === 'debate'
	) {

		if (!round.flip || round.sidelocks[section.id]) {
			if (parseInt(entry.side) === 1) {
				return `${round.settings.aff_label ? round.settings.aff_label.toUpperCase() : ' AFF'}`;
			}

			return `${round.settings.neg_label ? round.settings.neg_label.toUpperCase() : ' NEG'}`;
		}
		return 'FLIP';
	}

	if (round.eventType === 'speech') {
		return ordinalize(entry.speakerorder);
	}

	if (round.eventType === 'wudc') {
		if (!round.flip || round.sidelocks[section.id]) {
			return wudcPosition(entry.speakerorder);
		}
		return 'FLIP';
	}

	return '';
};
