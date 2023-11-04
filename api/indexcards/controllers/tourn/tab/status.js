/* eslint-disable no-continue */
import { showDateTime } from '@speechanddebate/nsda-js-utils';
import { flightTimes } from '../../../helpers/round';

export const sideCounts = {
	GET: async (req, res) => {

		const sideResults = await req.db.sequelize.query(`
			select
				ballot.judge, ballot.side,
				aff_label.value aff,
				neg_label.value neg

			from (panel, ballot, round)

				left join event_setting aff_label
					on aff_label.event = round.event
					and aff_label.tag = 'aff_label'

				left join event_setting neg_label
					on neg_label.event = round.event
					and neg_label.tag = 'neg_label'

			where round.id = :roundId
				and round.id = panel.round
				and panel.id = ballot.panel
				and ballot.bye != 1
				and ballot.forfeit != 1
				and panel.bye != 1
				and exists (
					select score.id
					from score
					where score.ballot = ballot.id
					and score.tag = 'winloss'
					and score.value = 1
				)
			group by ballot.judge
			order by ballot.side
		`, {
			replacements : { roundId: req.params.roundId },
			type         : req.db.sequelize.QueryTypes.SELECT,
		});

		const sideCounter = {};

		for (const result of sideResults) {
			if (result.side === 1) {
				if (!sideCounter[result.aff]) {
					sideCounter[result.aff] = 1;
				} else {
					sideCounter[result.aff]++;
				}
			} else if (result.side === 2) {
				if (!sideCounter[result.neg]) {
					sideCounter[result.neg] = 1;
				} else {
					sideCounter[result.neg]++;
				}
			}
		}

		res.status(200).json(sideCounter);

	},
};

export const attendance = {
	GET: async (req, res) => {
		const db = req.db;

		let queryLimit = '';

		if (req.params.timeslotId) {
			req.params.timeslotId = parseInt(req.params.timeslotId);
		}

		if (req.params.roundId) {
			req.params.roundId = parseInt(req.params.roundId);
		}

		if (req.params.timeslotId) {
			queryLimit = `where round.timeslot = ${req.params.timeslotId}`;
		} else if (req.params.roundId) {
			queryLimit = `where round.id = ${req.params.roundId}`;
		} else {
			return res.status(400).json({ message: 'No parameters sent for query' });
		}

		const attendanceQuery = `
			select
				cl.panel panel, cl.tag tag, cl.description description,
					CONVERT_TZ(cl.timestamp, '+00:00', tourn.tz) timestamp,
				person.id person,
				tourn.tz tz

			from panel, campus_log cl, tourn, person, round

			${queryLimit}

				and panel.round = round.id
				and panel.id = cl.panel
				and cl.tourn = tourn.id
				and cl.tag != 'observer'
				and cl.person = person.id

				and ( exists (
						select ballot.id
							from ballot, judge, entry
						where judge.id = ballot.judge
							and judge.person = person.id
							and ballot.panel = panel.id
							and ballot.entry = entry.id
							and entry.active = 1
					) or exists (
						select ballot.id
							from ballot, entry_student es, student, entry
						where ballot.panel = panel.id
							and ballot.entry = es.entry
							and es.student = student.id
							and student.person = person.id
							and ballot.entry = entry.id
							and entry.active = 1
					)
				)
			order by cl.timestamp
		`;

		const unlinkedAttendanceQuery = `
			select
				cl.panel panel, cl.tag tag, cl.description description,
					CONVERT_TZ(cl.timestamp, '+00:00', tourn.tz) timestamp,
				cl.student student, cl.judge judge,
				tourn.tz tz

			from panel, campus_log cl, tourn, round

			${queryLimit}

				and panel.round = round.id
				and panel.id = cl.panel
				and cl.tourn = tourn.id
				and cl.tag != 'observer'
				and (cl.person is NULL OR cl.person = 0)

				and ( exists (
						select ballot.id
							from ballot, entry
						where ballot.judge = cl.judge
							and ballot.panel = panel.id
							and ballot.entry = entry.id
							and entry.active = 1
					) or exists (
						select ballot.id
							from ballot, entry_student es, entry
						where ballot.panel = panel.id
							and ballot.entry = es.entry
							and es.student = cl.student
							and ballot.entry = entry.id
							and entry.active = 1
					)
				)
			order by cl.timestamp
		`;

		const entryAttendanceQuery = `
			select
				cl.panel panel, cl.tag tag, cl.description description,
					CONVERT_TZ(cl.timestamp, '+00:00', tourn.tz) timestamp,
				cl.entry entry, tourn.tz tz

			from panel, campus_log cl, tourn, round, ballot, entry

			${queryLimit}

				and panel.round = round.id
				and panel.id = cl.panel
				and cl.tourn = tourn.id
				and cl.entry = entry.id
				and cl.tag != 'observer'
				and panel.id = ballot.panel
				and ballot.entry = entry.id
				and entry.active = 1

			order by cl.timestamp
		`;

		const startsQuery = `
			select
				judge.person person, panel.id panel, panel.timestamp timestamp,
				CONVERT_TZ(ballot.judge_started, '+00:00', tourn.tz) startTime,
				ballot.audit audited,
				cl.tag,
				started_by.first startFirst, started_by.last startLast,
				judge.first judgeFirst, judge.last judgeLast,
				tourn.tz tz

			from (panel, tourn, round, ballot, event, judge, entry)

				left join person started_by on ballot.started_by = started_by.id
				left join campus_log cl on cl.panel = panel.id and cl.person = judge.person
					and cl.tag != 'observer'

			${queryLimit}

				and panel.round = round.id
				and round.event = event.id
				and event.tourn = tourn.id
				and ballot.panel = panel.id
				and ballot.judge = judge.id
				and ballot.judge_started > '1900-00-00 00:00:00'
				and ballot.entry = entry.id
				and entry.active = 1
			group by panel.id, judge.id
		`;

		const confirmedQuery = `
			select
				panel.id panel, panel.bye bye,
				CONVERT_TZ(confirmed_started.timestamp, '+00:00', tourn.tz) confirmedAt,
				CONCAT(confirmed_by.first,' ',confirmed_by.last) confirmedBy,
				tourn.tz tz

			from (panel, round, event, tourn)

				left join panel_setting confirmed_started
					on panel.id = confirmed_started.panel
					and confirmed_started.tag = 'confirmed_started'

				left join person confirmed_by
					on confirmed_by.id = confirmed_started.value

			${queryLimit}
				and round.id = panel.round
				and round.event = event.id
				and event.tourn = tourn.id
		`;

		// A raw query to go through the category filter
		const [attendanceResults]         = await db.sequelize.query(attendanceQuery);
		const [unlinkedAttendanceResults] = await db.sequelize.query(unlinkedAttendanceQuery);
		const [entryAttendanceResults]    = await db.sequelize.query(entryAttendanceQuery);
		const [startsResults]             = await db.sequelize.query(startsQuery);
		const [confirmedResults]          = await db.sequelize.query(confirmedQuery);

		const status = {
			panel   : {},
			person  : {},
			entry   : {},
			judge   : {},
			student : {},
		};

		attendanceResults.forEach( attend => {

			if (status.person[attend.person] === undefined) {
				status.person[attend.person] = {};
			}

			status.person[attend.person][attend.panel] = {
				tag         : attend.tag,
				timestamp   : attend.timestamp.toJSON,
				description : attend.description,
			};
		});

		confirmedResults.forEach( confirmation => {
			if (confirmation.bye) {
				status.panel[confirmation.panel] = `Bye rounds don't need confirmation, silly`;
			} else if (confirmation.confirmedBy) {
				const timestamp = showDateTime(confirmation.confirmedAt, { tz: confirmation.tz, format: 'daytime' });
				status.panel[confirmation.panel] = `Confirmed by ${confirmation.confirmedBy} at ${timestamp}`;
			}
		});

		unlinkedAttendanceResults.forEach( attend => {

			if (attend.student) {
				status.student[attend.student] = {
					[attend.panel]  : {
						tag         : attend.tag,
						timestamp   : attend.timestamp.toJSON,
						description : attend.description,
						started     : showDateTime(attend.timestamp, { tz: attend.tz, format: 'daytime' }),
					},
				};
			} else if (attend.judge) {
				status.judge[attend.judge] = {
					[attend.panel]  : {
						tag         : attend.tag,
						timestamp   : attend.timestamp.toJSON,
						description : attend.description,
						started     : showDateTime(attend.timestamp, { tz: attend.tz, format: 'daytime' }),
					},
				};
			} else if (attend.person) {
				status.person[attend.person] = {
					[attend.panel]  : {
						tag         : attend.tag,
						timestamp   : attend.timestamp.toJSON,
						description : attend.description,
						started     : showDateTime(attend.timestamp, { tz: attend.tz, format: 'daytime' }),
					},
				};
			}
		});

		entryAttendanceResults.forEach( attend => {
			status.entry[attend.entry] = {
				[attend.panel]  : {
					tag         : attend.tag,
					timestamp   : attend.timestamp.toJSON,
					description : attend.description,
					started     : showDateTime(attend.timestamp, { tz: attend.tz, format: 'daytime' }),
				},
			};
		});

		startsResults.forEach( start => {

			if (status.person[start.person] === undefined) {
				status.person[start.person] = {};
			}

			if (status.person[start.person][start.panel] === undefined) {
				status.person[start.person][start.panel] = {};
			}

			const myPanel = status.person[start.person][start.panel];

			if (start.startFirst === undefined) {
				myPanel.started_by = `${start.judgeFirst}  ${start.judgeLast}`;
			} else {
				myPanel.started_by = `${start.startFirst} ${start.startLast}`;
			}

			if (start.startTime) {
				myPanel.started = showDateTime(start.startTime, { tz: start.tz, format: 'daytime' });
			}

			myPanel.timestamp = start.timestamp;

			if (start.tag) {
				myPanel.tag = start.tag;
			} else {
				myPanel.tag = 'absent';
			}

			if (start.audited) {
				myPanel.audited = true;
			}
		});

		if (status.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		}

		return res.status(200).json(status);
	},

	POST: async (req, res) => {

		try {

			const now = Date();
			const db = req.db;

			const targetType = req.body.target_type;
			const targetId = req.body.target_id;
			let target = '';

			if (targetType === 'student') {
				target = await db.student.findByPk(targetId);
			} else if (targetType === 'entry') {
				target = await db.entry.findByPk(targetId);
			} else if (targetType === 'judge') {
				target = await db.judge.findByPk(targetId);
			} else {
				target = await db.person.findByPk(targetId);
			}

			if (!target) {
				return res.status(201).json({
					error   : true,
					message : `No person to mark present for ID ${target} ${targetType} ${req.body.target_id}`,
				});
			}

			const panel = await db.panel.findByPk(req.body.panel);

			if (!panel) {
				return res.status(201).json({
					error   : true,
					message : `No section found for ID ${req.body.panel}`,
				});
			}

			if (req.body.setting_name === 'judge_started') {

				let judge = {};

				if (targetType === 'judge') {
					judge = target;
				} else {
					judge = await db.judge.findByPk(req.body.judge);
				}

				if (parseInt(req.body.property_name) > 0) {

					const eraseStart = `update ballot set started_by = NULL,
						judge_started = NULL
						where judge = :judgeId
						and panel = :panelId `;

					await db.sequelize.query(eraseStart, {
						replacements: { judgeId: judge.id, panelId: panel.id },
					});

					const response = {
						error : false,
						reclass: [
							{   id		  : `${panel.id}_${targetId}_start`,
								removeClass : 'greentext',
								addClass	: 'yellowtext',
							},{
								id		  : `${panel.id}_${targetId}_start`,
								removeClass : 'fa-star',
								addClass	: 'fa-stop',
							},
						],
						reprop: [
							{   id		  : `start_${panel.id}_${targetId}`,
								property	: 'property_name',
								value 		: false,
							},{
								id		  : `start_${panel.id}_${targetId}`,
								property	: 'title',
								value 		: 'Not started',
							},
						],
						message : 'Judge marked as not started',
					};

					return res.status(201).json(response);
				}

				await db.ballot.update({
					started_by    : req.session.person,
					judge_started : now,
				},{
					where : {
						panel : panel.id,
						judge : judge.id,
					},
				});

				const response = {
					error : false,
					reclass: [
						{   id		  : `${panel.id}_${targetId}_start`,
							addClass	: 'greentext',
							removeClass : 'yellowtext',
						},{
							id		  : `${panel.id}_${targetId}_start`,
							addClass	: 'fa-star',
							removeClass : 'fa-stop',
						},
					],
					reprop: [
						{   id		  : `start_${panel.id}_${targetId}`,
							property	: 'property_name',
							value 		: 1,
						},{
							id		  : `start_${panel.id}_${targetId}`,
							property	: 'title',
							value 		: `Judge marked as started by ${req.session.name}`,
						},
					],
					message : `Judge marked as started by ${req.session.name}`,
				};

				return res.status(201).json(response);
			}

			if (parseInt(req.body.property_name) === 1) {

				// The property already being 1 means that they're currently
				// present, so mark them as absent.

				let logMessage;

				if (target.first) {
					logMessage = `${target.first} ${target.last} marked as absent by ${req.session.email}`;
				} else if (target.code) {
					logMessage = `${target.code} marked as absent by ${req.session.email}`;
				}

				const log = {
					tag         : 'absent',
					description : logMessage,
					tourn       : req.params.tourn_id,
					panel       : panel.id,
				};

				if (targetType === 'student') {
					log.student = target.id;
				} else if (targetType === 'entry') {
					log.entry = target.id;
				} else if (targetType === 'judge') {
					log.judge = target.id;
				} else {
					log.person = target.id;
				}

				await db.campusLog.create(log);

				// Oh for the days I have react going and don't need to do the
				// following nonsense

				return res.status(201).json({
					error   : false,
					message : logMessage,
					reclass : [
						{	id          : targetType ? `${panel.id}_${targetType}_${targetId}` : `${panel.id}_${targetId}`,
							removeClass : 'greentext',
							addClass    : 'brightredtext',
						},
						{	id          : targetType ? `${panel.id}_${targetType}_${targetId}` : `${panel.id}_${targetId}`,
							removeClass : 'fa-check',
							addClass    : 'fa-circle',
						},
					],
					reprop  : [
						{	id       : `container_${panel.id}_${targetId}`,
							property : 'property_name',
							value    : false,
						},
					],
				});
			}

			// In this case they're currently marked absent, so we mark them
			// present

			let logMessage;
			if (target.first) {
				logMessage = `${target.first} ${target.last} marked as present by ${req.session.email}`;
			} else if (target.code) {
				logMessage = `${target.code} marked as present by ${req.session.email}`;
			}

			const log = {
				tag         : 'present',
				description : logMessage,
				tourn       : req.params.tourn_id,
				panel       : panel.id,
			};

			if (targetType === 'student') {
				log.student = target.id;
			} else if (targetType === 'entry' || req.body.setting_name === 'offline_entry') {
				log.entry = target.id;
			} else if (targetType === 'judge') {
				log.judge = target.id;
			} else {
				log.person = target.id;
			}

			await db.campusLog.create(log);

			return res.status(201).json({
				error   : false,
				message : logMessage,
				reclass : [
					{	id          : targetType ? `${panel.id}_${targetType}_${targetId}` : `${panel.id}_${targetId}`,
						addClass	: 'greentext',
						removeClass : 'brightredtext',
					},
					{	id          : targetType ? `${panel.id}_${targetType}_${targetId}` : `${panel.id}_${targetId}`,
						addClass	: 'fa-check',
						removeClass : 'fa-circle',
					},
				],
				reprop  : [
					{	id	   : `container_${panel.id}_${targetId}`,
						property : 'property_name',
						value	: 1,
					},
				],
			});

		} catch (err) {
			// errorLogger.info(err);
		}
	},
};

function addZero(i) {
	if (i < 10) {
		i = `0${i}`;
	}
	return i;
}

export const schematStatus = {

	GET: async (req, res) => {
		const db = req.db;

		const labels = await db.sequelize.query(`
			select
				SUBSTRING(aff_label.value, 1, 1) aff,
				SUBSTRING(neg_label.value, 1, 1) neg
			from event_setting aff_label, event_setting neg_label, round

			where round.id = :roundId
				and round.event = aff_label.event
				and round.event = neg_label.event
				and aff_label.tag = 'aff_label'
				and neg_label.tag = 'neg_label'
		`, {
			replacements: { roundId: req.params.roundId },
			type         : db.sequelize.QueryTypes.SELECT,
		});

		const tmplabel = labels.shift();

		const label = {
			1: tmplabel?.aff || 'A',
			2: tmplabel?.neg || 'N',
		};

		const rawBallots = await db.sequelize.query(`
			select
				ballot.id ballot,
				panel.id panel,
				judge.id judge,
				ballot.chair,
				CONVERT_TZ(ballot.judge_started, '+00:00', tourn.tz) startTime,
				ballot.audit,
				ballot.side,
				ballot.bye, ballot.forfeit, panel.bye pbye,
				rank.id rank,
				point.id point,
				winloss.id winloss,
				winloss.value winner,
				rubric.id rubric,
				panel.flight

			from (judge, ballot, panel, round, event, tourn)
				left join score rank on rank.ballot = ballot.id and rank.tag = 'rank'
				left join score point on point.ballot = ballot.id and point.tag = 'point'
				left join score winloss on winloss.ballot = ballot.id and winloss.tag = 'winloss'
				left join score rubric on rubric.ballot = ballot.id and rubric.tag = 'rubric'

			where round.id = :roundId
				and panel.round = round.id
				and panel.id = ballot.panel
				and ballot.judge = judge.id
				and round.event = event.id
				and event.tourn = tourn.id
		`, {
			replacements: { roundId: req.params.roundId },
			type         : db.sequelize.QueryTypes.SELECT,
		});

		const round = {
			judges : {},
			out    : {},
			panels : {},
		};

		rawBallots.forEach( (ballot) => {

			if (!round.judges[ballot.judge]) {
				round.judges[ballot.judge] = {};
			}
			if (!round.judges[ballot.judge][ballot.flight]) {
				round.judges[ballot.judge][ballot.flight] = { panel: ballot.panel };
			}

			const judge = round.judges[ballot.judge][ballot.flight];

			if (!round.out[ballot.flight]) {
				round.out[ballot.flight] = {};
			}

			if (ballot.audit) {

				if (!judge.text) {
					judge.text = '';
				}

				if (!round.panels[ballot.panel]) {
					round.panels[ballot.panel] = 10;
				} else {
					round.panels[ballot.panel] += 10;
				}

				if (ballot.winloss) {
					if (ballot.winner) {
						judge.text = label[ballot.side];
						judge.class = 'greentext semibold';
					}
				} else if (ballot.pbye) {
					judge.text = 'BYE';
					judge.class = 'graytext semibold';
				} else if (ballot.bye) {
					if (judge.text) {
						judge.text += `/`;
					}
					judge.text += `Bye`;
					judge.class = 'graytext semibold';
				} else if (ballot.forfeit) {
					if (judge.text) {
						judge.text += `/`;
					}
					judge.text += `Fft`;
					judge.class = 'graytext semibold';
				} else if (ballot.rank) {
					judge.text = 'in';
					judge.class = 'greentext semibold';
				} else if (ballot.chair) {
					judge.class = 'fa fa-sm fa-star greentext';
					judge.text = '-';
				}
			} else if (ballot.pbye) {
				round.panels[ballot.panel] = 100;
				judge.text = 'BYE';
			} else if (ballot.winloss || ballot.rank || ballot.point || ballot.rubric ) {
				round.out[ballot.flight][ballot.judge] = true;
				round.panels[ballot.panel] = 100;
				judge.text = '&frac12;';
				judge.class = 'redtext';
			} else if (ballot.startTime) {
				round.out[ballot.flight][ballot.judge] = true;
				round.panels[ballot.panel] = 100;
				const started = new Date(ballot.startTime);
				judge.text = `${started.getUTCHours()}:${addZero(started.getUTCMinutes())}`;
			} else {
				round.out[ballot.flight][ballot.judge] = true;
				delete round.judges[ballot.judge][ballot.flight];
			}
		});

		res.status(200).json(round);
	},
};

export const eventStatus = {
	GET: async (req, res) => {
		const db = req.db;

		const eventQuery = `
			select
				event.id, event.name, event.abbr,
				round.id roundId, round.name round_name, round.label, round.flighted, round.type,
				round.start_time round_start,
				timeslot.start timeslot_start,
				panel.id panel_id, panel.flight,
				ballot.judge,
					ballot.audit, ballot.judge_started,
				score.id score_id, score.tag,
				flight_offset.value flight_offset,
				tourn.tz tz

			from (round, panel, ballot, event, tourn, timeslot, entry)

				left join score on score.ballot = ballot.id
					and score.tag in ('winloss', 'point', 'rank')

				left join event_setting flight_offset
					on flight_offset.event = event.id
					and flight_offset.tag = 'flight_offset'

			where round.event = event.id

				and event.tourn = :tournId

				and round.id = panel.round
				and panel.id = ballot.panel
				and round.event = event.id
				and event.tourn = tourn.id
				and round.timeslot = timeslot.id
				and ballot.entry = entry.id
				and entry.active = 1

				and panel.bye      = 0
				and ballot.bye     = 0
				and ballot.forfeit = 0
				and ballot.judge > 0

				and exists (
					select b2.id
						from ballot b2, panel p2, entry e2
					where b2.panel = p2.id
						and p2.round   = round.id
						and p2.bye     = 0
						and b2.bye     = 0
						and b2.audit   = 0
						and b2.forfeit = 0
						and b2.judge   > 0
						and b2.entry  = e2.id
						and e2.active = 1
				)

			order by event.name, round.name
		`;

		const eventResults = await db.sequelize.query(eventQuery, {
			replacements: { tournId:  req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});
		const status         = {};
		const statusCache    = { done_judges: {} };
		const tournPerms     = req.session[req.params.tourn_id];

		eventResults.forEach( event => {

			if (
				tournPerms.level !== 'owner'
				&& tournPerms.level !== 'tabber'
				&& !tournPerms.events[event.id]
			) {
				return;
			}

			event.flighted = event.flighted || 1;
			event.flight = event.flight || 1;
			event.round_start = event.round_start || event.timeslot_start;

			// I feel like if there's not a simple dynamic way to do this in JS
			// that's a reason to take another look at Python.

			if (!statusCache.done_judges[`${event.roundId}-${event.judge}-${event.flight}`]) {
				statusCache.done_judges[`${event.roundId}-${event.judge}-${event.flight}`] = true;

				if (typeof status[event.id] === 'undefined') {
					status[event.id] = {
						abbr   : event.abbr,
						name   : event.name,
						rounds : {},
					};
				}

				if (typeof status[event.id].rounds[event.roundId] === 'undefined') {
					status[event.id].rounds[event.roundId] = {
						number   : event.round_name,
						flighted : event.flighted,
						type     : event.type,
						label    : event.label,
					};

					if (status[event.id].rounds[event.roundId].label === '') {
						status[event.id].rounds[event.roundId].label = `Rnd ${event.round_name}`;
					}
				}

				if (typeof status[event.id].rounds[event.roundId][event.flight] === 'undefined') {

					if (typeof status[event.id].rounds[event.roundId][event.flight] === 'undefined') {
						// this can't seriously be how this works.
						status[event.id].rounds[event.roundId][event.flight] = {};
					}

					status[event.id]
						.rounds[event.roundId][event.flight]
						.start_time
						= new Date(event.round_start).toLocaleTimeString(
								'en-us', {
									hour     : 'numeric',
									minute   : '2-digit',
									timeZone : event.tz,
								});
				}

				if (typeof statusCache[event.id] === 'undefined') {
					statusCache[event.id] = {};
				}

				if (event.bye
					|| event.forfeit
					|| event.audit
					|| event.bbye
				) {

					// THIS IS SUCH A HORRIFIC ANTIPATTERN I BETTER BE WRONG ABOUT THIS
					if (status[event.id].rounds[event.roundId][event.flight].complete) {
						status[event.id].rounds[event.roundId][event.flight].complete++;
					} else {
						status[event.id].rounds[event.roundId][event.flight].complete = 1;
					}

					if (statusCache[event.id].finished) {
						statusCache[event.id].finished++;
					} else {
						statusCache[event.id].finished = 1;
					}

				} else {

					status[event.id].rounds[event.roundId][event.flight].undone = true;

					if (event.score_id) {

						status[event.id].rounds[event.roundId][event.flight].scored =
							++status[event.id].rounds[event.roundId][event.flight].scored || 1;

						status[event.id].rounds[event.roundId].in_progress = true;
						statusCache[event.id].pending = ++statusCache[event.id].pending || 1;

					} else if (event.judge_started) {

						status[event.id].rounds[event.roundId][event.flight].started =
							++status[event.id].rounds[event.roundId][event.flight].started || 1;

						status[event.id].rounds[event.roundId].in_progress = true;

						statusCache[event.id].pending = ++statusCache[event.id].pending || 1;

					} else {

						status[event.id].rounds[event.roundId][event.flight].unstarted =
							++status[event.id].rounds[event.roundId][event.flight].unstarted
							|| 1;
					}
				}
			}
		});

		Object.keys(status).forEach(eventId => {

			const sortByRound = arr => {
				arr.sort((b, a) => {
					return parseInt(
						status[eventId].rounds[a].number) - parseInt(status[eventId].rounds[b].number
					);
				});
			};

			const rounds = Object.keys(status[eventId].rounds);
			sortByRound(rounds);

			rounds.forEach( roundId => {

				if (statusCache[eventId].pending) {

					if (!status[eventId].rounds[roundId].in_progress) {
						delete status[eventId].rounds[roundId];
					}
				} else {

					if (status[eventId].rounds[roundId].unstarted) {

						if (!statusCache[eventId].first_unstarted) {
							statusCache[eventId].first_unstarted = roundId;
						} else {
							delete status[eventId].rounds[roundId];
						}

					} else {

						if (statusCache[eventId].first_unstarted) {
							delete status[eventId].rounds[roundId];
						} else {

							if (statusCache[eventId].last_done) {
								delete status[eventId].rounds[statusCache[eventId].last_done];
							}

							statusCache[eventId].last_done = roundId;
						}
					}
				}
			});
		});

		const lastQuery = `
			select
				event.id, event.name, event.abbr,
				round.id roundId, round.name round_name, round.label, round.type,
				round.start_time round_start,
				timeslot.start timeslot_start,
				tourn.tz tz
			from round, panel, event, timeslot, tourn

			where round.event = event.id
				and event.tourn = ${req.params.tourn_id}
				and round.id = panel.round
				and round.name = (
					select max(r2.name)
						from round r2, panel p2
						where r2.event = event.id
						and r2.id = p2.round
				)
				and round.timeslot = timeslot.id
				and timeslot.tourn = tourn.id
			group by event.id
			order by round.name
		`;

		const [lastResults]  = await db.sequelize.query(lastQuery);

		lastResults.forEach( event => {

			if (status[event.id]) {
				return;
			}

			event.flighted = event.flighted || 1;
			event.flight = event.flight || 1;

			if (!status[event.id]) {
				status[event.id] = {
					abbr   : event.abbr,
					name   : event.name,
					rounds : {},
				};
			}

			if (!status[event.id].rounds[event.roundId]) {
				status[event.id].rounds[event.roundId] = {
					number   : event.round_name,
					flighted : event.flighted,
					type     : event.type,
					label    : event.label,
				};

				if (status[event.id].rounds[event.roundId].label === '') {
					status[event.id].rounds[event.roundId].label = `Rnd ${event.round_name}`;
				}
			}

			event.round_start = event.round_start || event.timeslot_start;

			if (!status[event.id].rounds[event.roundId][event.flight]) {

				if (!status[event.id].rounds[event.roundId][event.flight]) {
					// this can't seriously be how this works.
					status[event.id].rounds[event.roundId][event.flight] = {};
				}

				status[event.id]
					.rounds[event.roundId][event.flight]
					.start_time
					= new Date(event.round_start).toLocaleTimeString(
							'en-us', {
								hour         : 'numeric',
								minute       : '2-digit',
								timeZone     : event.tz,
							});
			}

			if (!statusCache[event.id]) {
				statusCache[event.id] = {};
			}

		});

		if (status.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		}

		return res.status(200).json(status);
	},
};

export const eventDashboard = {

	GET: async (req, res) => {

		const db = req.db;
		const eventQuery = `
			select
				event.id event_id, event.name event_name, event.abbr event_abbr,
				round.id roundId, round.name round_name, round.type round_type,
				round.label, round.flighted, round.type,
				round.start_time round_start,
				timeslot.start timeslot_start,
				panel.id panel, panel.flight,
				ballot.id ballot, ballot.judge judge,
					ballot.audit, ballot.judge_started,
				score.id score_id, score.tag

			from (round, panel, ballot, event, tourn, timeslot, entry)

				left join score on score.ballot = ballot.id
					and score.tag in ('winloss', 'point', 'rank')

			where round.event = event.id

				and event.tourn     = :tournId
				and round.id        = panel.round
				and panel.id        = ballot.panel
				and round.event     = event.id
				and event.tourn     = tourn.id
				and round.timeslot  = timeslot.id
				and ballot.entry    = entry.id
				and entry.active    = 1
				and round.published = 1
				and panel.bye       = 0
				and ballot.bye      = 0
				and ballot.forfeit  = 0
				and ballot.judge    > 0

				and exists (
					select b2.id
						from ballot b2, panel p2, entry e2
					where b2.panel     = p2.id
						and p2.round   = round.id
						and p2.bye     = 0
						and b2.bye     = 0
						and (b2.audit  = 0 OR round.type = 'final')
						and b2.forfeit = 0
						and b2.entry   = e2.id
						and e2.active  = 1
						and b2.judge   > 0
				)
			order by event.name, round.name, ballot.judge, ballot.audit
		`;

		const statusResults = await db.sequelize.query(eventQuery, {
			replacements: { tournId: req.params.tourn_id },
			type         : db.sequelize.QueryTypes.SELECT,
		});

		const status = { done: {} };
		const tournPerms = req.session[req.params.tourn_id];

		for await (const result of statusResults) {

			// Check that I can see this event.
			if (
				tournPerms.level !== 'owner'
				&& tournPerms.level !== 'tabber'
				&& !tournPerms.events?.[result.event]
			) {
				continue;
			}

			// Judges have more than one ballot per section so stop if we've
			// seen you before

			if (status.done[result.panel]?.[result.judge]) {
				continue;
			}

			// If the round isn't on the status board already, create an object
			// for it

			if (!status[result.roundId]) {

				const times = await flightTimes(result.roundId);
				const numFlights = result.flighted || 1;

				if (result.flight > numFlights) {
					result.flight = numFlights;
				}

				status[result.roundId] = {
					eventId   : result.event_id,
					eventName : result.event_abbr,
					roundId   : result.roundId,
					number    : result.round_name,
					name      : result.label ? result.label : `Rd ${result.round_name}`,
					type      : result.round_type,
					undone    : false,
					started   : false,
					flights   : {},
				};

				for (let f = 1; f <= numFlights; f++) {
					status[result.roundId].flights[f] = {
						done      : 0,
						half      : 0,
						started   : 0,
						nada      : 0,
						...times[f],
					};
				}
			}

			// Prevent future duplicates.  This incidentally is why the sql
			// query sorts by ballot.audit; unaudited ballots come first and
			// won't slip through.

			if (status.done[result.panel]) {
				status.done[result.panel][result.judge] = true;
			} else {
				status.done[result.panel] = {
					[result.judge] : true,
				};
			}

			if (result.audit) {

				// Is the ballot done?
				status[result.roundId].flights[result.flight].done++;
				status[result.roundId].started = true;

			} else if (status[result.roundId]?.flights?.[result.flight]) {

				status[result.roundId].undone = true;

				if (result.score_id) {
					// Does the ballot have scores?
					status[result.roundId].flights[result.flight].half++;
					status[result.roundId].started = true;
				} else if (result.judge_started) {
					status[result.roundId].flights[result.flight].started++;
					status[result.roundId].started = true;
				} else {
					status[result.roundId].flights[result.flight].nada++;
				}
			}
		}

		delete status.done;

		for await (const roundId of Object.keys(status)) {
			if (!status[roundId].started || !status[roundId].undone) {
				delete status[roundId];
			}
		}
		return res.status(200).json(status);
	},
};

attendance.GET.apiDoc = {
	summary: 'Room attedance and start status of a round or timeslot',
	operationId: 'roundStatus',
	parameters: [
		{
			in		  : 'path',
			name		: 'tourn_id',
			description : 'Tournament ID',
			required	: false,
			schema	  : {
				type	: 'integer',
				minimum : 1,
			},
		},{
			in          : 'path',
			name        : 'roundId',
			description : 'Round ID',
			required    : false,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},
	],
	responses: {
		200: {
			description: 'Status Data',
			content: {
				'*/*': {
					schema: {
						type: 'object',
						items: { $ref: '#/components/schemas/Event' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['tourn/tab'],
};
