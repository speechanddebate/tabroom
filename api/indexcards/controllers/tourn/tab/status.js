import { showDateTime } from '../../../helpers/common.js';

export const attendance = {

	GET: async (req, res) => {

		const db = req.db;

		let queryLimit = '';

		if (req.params.timeslot_id) {
			req.params.timeslot_id = parseInt(req.params.timeslot_id);
		}

		if (req.params.round_id) {
			req.params.round_id = parseInt(req.params.round_id);
		}

		if (req.params.timeslot_id) {
			queryLimit = `where round.timeslot = ${req.params.timeslot_id}`;
		} else if (req.params.round_id) {
			queryLimit = `where round.id = ${req.params.round_id}`;
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
				and cl.person = person.id

				and ( exists (
						select ballot.id
							from ballot, judge
						where judge.id = ballot.judge
							and judge.person = person.id
							and ballot.panel = panel.id
					) or exists (
						select ballot.id
							from ballot, entry_student es, student
						where ballot.panel = panel.id
							and ballot.entry = es.entry
							and es.student = student.id
							and student.person = person.id
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
				and (cl.person is NULL OR cl.person = 0)

				and ( exists (
						select ballot.id
							from ballot
						where ballot.judge = cl.judge
							and ballot.panel = panel.id
					) or exists (
						select ballot.id
							from ballot, entry_student es
						where ballot.panel = panel.id
							and ballot.entry = es.entry
							and es.student = cl.student
					)
				)
			order by cl.timestamp
		`;

		const startsQuery = `
			select
				judge.person person, panel.id panel, panel.timestamp timestamp,
				ballot.judge_started startTime,
				ballot.audit audited,
				started_by.first startFirst, started_by.last startLast,
				judge.first judgeFirst, judge.last judgeLast,
				tourn.tz tz

			from (panel, tourn, round, ballot, event, judge)

				left join person started_by on ballot.started_by = started_by.id

			${queryLimit}

				and panel.round = round.id
				and round.event = event.id
				and event.tourn = tourn.id
				and ballot.panel = panel.id
				and ballot.judge = judge.id
				and ballot.judge_started > '1900-00-00 00:00:00'
			group by panel.id, judge.id
		`;

		// A raw query to go through the category filter
		const [attendanceResults]         = await db.sequelize.query(attendanceQuery);
		const [unlinkedAttendanceResults] = await db.sequelize.query(unlinkedAttendanceQuery);
		const [startsResults]             = await db.sequelize.query(startsQuery);

		const status = {};

		attendanceResults.forEach( attend => {
			
			if (status[attend.person] === undefined) { 
				status[attend.person] = {};
			}

			status[attend.person][attend.panel] = {
				tag         : attend.tag,
				timestamp   : attend.timestamp.toJSON,
				description : attend.description,
				started     : showDateTime(attend.timestamp, { tz: attend.tz, format: 'daytime' })
			};
		});

		unlinkedAttendanceResults.forEach( attend => {

			if (attend.student) {
				attend.person = `student_${attend.student}`;
			} else if (attend.judge) {
				attend.person = `judge_${attend.judge}`;
			}
			
			if (attend.person) {
				status[attend.person] = {
					[attend.panel]  : {
						tag         : attend.tag,
						timestamp   : attend.timestamp.toJSON,
						description : attend.description,
						started     : showDateTime(attend.timestamp, {tz: attend.tz, format: 'daytime' })
					},
				};
			}
		});

		startsResults.forEach( start => {
			if (status[start.person] === undefined) {
				status[start.person] = {};
			}

			if (status[start.person][start.panel] === undefined) {
				status[start.person][start.panel] = {};
			}

			if (start.startFirst === undefined) { 
				status[start.person][start.panel].started_by = start.judgeFirst+' '+start.judgeLast;
			} else {
				status[start.person][start.panel].started_by = start.startFirst+' '+start.startLast;
			}

			status[start.person][start.panel].started = showDateTime(
				start.startTime,
				{ tz: start.tz, format: 'daytime' }
			);

			status[start.person][start.panel].timestamp = start.timestamp;

			if (start.audited) {
				status[start.person][start.panel].audited = true;
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

			let [targetType, targetId] = req.body.target_id.toString().split('_');
			let target;

			if (targetType === 'student') {
				target = await db.student.findByPk(targetId);
			} else if (targetType === 'judge') {
				target = await db.judge.findByPk(targetId);
			} else {
				target = await db.person.findByPk(req.body.target_id);
			}

			targetId = req.body.target_id;

			if (!target) {
				return res.status(201).json({
					error   : true,
					message : `No person to mark present for ID ${req.body.target_id}`,
				});
			}

			const panel = await db.panel.findByPk(req.body.related_thing);

			if (!panel) {
				return res.status(201).json({
					error   : true,
					message : `No section found for ID ${req.body.related_thing}`,
				});
			}

			if (req.body.setting_name === 'judge_started') {

				let judge = {};

				if (targetType === 'judge') {
					judge = target;
				} else {
					judge = await db.judge.findByPk(req.body.another_thing);
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
					started_by: req.session.person,
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

				const logMessage = `${target.first} ${target.last} marked as absent by ${req.session.email}`;

				const log = {
					tag         : 'absent',
					description : logMessage,
					tourn       : req.params.tourn_id,
					panel       : panel.id,
				};

				if (targetType === 'student') {
					log.student = target.id;
				} else if (targetType === 'judge') {
					log.judge = target.id;
				} else {
					log.person = target.id;
				}

				targetType = 'stfu_linter';

				if (req.body.setting_name === 'entry') {
					log.entry = req.body.another_thing;
				} else if (req.body.setting_name === 'judge') {
					log.judge = req.body.another_thing;
				}

				await db.campusLog.create(log);

				// Oh for the days I have react going and don't need to do the
				// following nonsense

				return res.status(201).json({
					error   : false,
					message : logMessage,
					reclass : [
						{	id          : `${panel.id}_${targetId}`,
							removeClass : 'greentext',
							addClass    : 'brightredtext',
						},
						{	id          : `${panel.id}_${targetId}`,
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

			const logMessage = `${target.first} ${target.last} marked as present by ${req.session.email}`;

			const log = {
				tag         : 'present',
				description : logMessage,
				tourn       : req.params.tourn_id,
				panel       : panel.id,
			};

			if (targetType === 'student') {
				log.student = target.id;
			} else if (targetType === 'judge') {
				log.judge = target.id;
			} else {
				log.person = target.id;
			}

			if (req.body.setting_name === 'entry') {
				log.entry = req.body.another_thing;
			} else if (req.body.setting_name === 'judge') {
				log.judge = req.body.another_thing;
			}

			await db.campusLog.create(log);

			return res.status(201).json({
				error   : false,
				message : logMessage,
				reclass : [
					{	id		  : `${panel.id}_${req.body.target_id}`,
						addClass	: 'greentext',
						removeClass : 'brightredtext',
					},
					{	id		  : `${panel.id}_${req.body.target_id}`,
						addClass	: 'fa-check',
						removeClass : 'fa-circle',
					},
				],
				reprop  : [
					{	id	   : `container_${panel.id}_${req.body.target_id}`,
						property : 'property_name',
						value	: 1,
					},
				],
			});

		} catch (err) {
			console.log(err);
		}
	},
};

export const eventStatus = {

	GET: async (req, res) => {

		const db = req.db;
		const perms = req.session.perms;

		let queryLimit = '';

		const eventQuery = `
			select
				event.id, event.name, event.abbr,
				round.id round_id, round.name round_name, round.label, round.flighted, round.type,
				round.start_time round_start,
				timeslot.start timeslot_start,
				panel.id panel_id, panel.bye, panel.flight,
				ballot.id ballot_id, ballot.judge, ballot.bye bbye, ballot.forfeit bfft,
					ballot.audit, ballot.judge_started,
				score.id score_id, score.tag,
				flight_offset.value offset,
				tourn.tz tz

			from (round, panel, ballot, event, tourn, timeslot)

				left join score on score.ballot = ballot.id
					and score.tag in ('winloss', 'point', 'rank')

				left join event_setting flight_offset
					on flight_offset.event = event.id
					and flight_offset.tag = 'flight_offset'

			where round.event = event.id

				and event.tourn = ${req.params.tourn_id}

				and round.id = panel.round
				and panel.id = ballot.panel
				and round.event = event.id
				and event.tourn = tourn.id
				and round.timeslot = timeslot.id

				and panel.bye      = 0
				and ballot.bye     = 0
				and ballot.forfeit = 0
				and ballot.judge > 0

				and exists (
					select b2.id
						from ballot b2, panel p2
						where b2.panel = p2.id
						and p2.round   = round.id
						and p2.bye     = 0
						and b2.bye     = 0
						and b2.audit   = 0
						and b2.forfeit = 0
						and b2.judge   > 0
				)

			order by event.name, round.name
		`;

		const [eventResults] = await db.sequelize.query(eventQuery);
		const status         = {};
		const statusCache    = { done_judges: {} };
		const tournPerms     = req.session[req.params.tourn_id];

		eventResults.forEach( event => {

			if (
				tournPerms.level === 'owner'
				|| tournPerms.level === 'tabber'
				|| tournPerms.events[event.id]
			) {
				// I am just A-OK!
			} else {
				return;
			}

			event.flighted = event.flighted || 1;
			event.flight = event.flight || 1;
			event.round_start = event.round_start || event.timeslot_start;

			// I feel like if there's not a simple dynamic way to do this in JS
			// that's a reason to take another look at Python.

			if (statusCache.done_judges[event.round_id+'-'+event.judge+'-'+event.flight]) {

				return;

			} else {

				statusCache.done_judges[event.round_id+'-'+event.judge+'-'+event.flight] = true;

				if (status[event.id] == undefined) {
					status[event.id] = {
						abbr   : event.abbr,
						name   : event.name,
						rounds : {}
					};
				}

				if (status[event.id].rounds[event.round_id] == undefined) {
					status[event.id].rounds[event.round_id] = {
						number   : event.round_name,
						flighted : event.flighted,
						type     : event.type,
						label    : event.label,
					}

					if (status[event.id].rounds[event.round_id].label == '') {
						status[event.id].rounds[event.round_id].label = `Rnd ${ event.round_name }`;
					}
				}

				if (status[event.id].rounds[event.round_id][event.flight] == undefined) {

					if (status[event.id].rounds[event.round_id][event.flight] == undefined) {
						// this can't seriously be how this works.
						status[event.id].rounds[event.round_id][event.flight] = {};
					}

					status[event.id]
						.rounds[event.round_id][event.flight]
						.start_time
						= new Date(event.round_start).toLocaleTimeString(
							'en-us', {
								hour     : 'numeric',
								minute   : '2-digit',
								timeZone : event.tz
							});
				}

				if (statusCache[event.id] == undefined) {
					statusCache[event.id] = {};
				}

				if (event.bye
					|| event.forfeit
					|| event.audit
					|| event.bbye
				) {

					//THIS IS SUCH A HORRIFIC ANTIPATTERN I BETTER BE WRONG ABOUT THIS
					if (status[event.id].rounds[event.round_id][event.flight].complete) {
						status[event.id].rounds[event.round_id][event.flight].complete++;
					} else {
						status[event.id].rounds[event.round_id][event.flight].complete = 1;
					}

					if (statusCache[event.id].finished) {
						statusCache[event.id].finished++;
					} else {
						statusCache[event.id].finished = 1;
					}

				} else {

					status[event.id].rounds[event.round_id][event.flight].undone = true;

					if (event.score_id) {

						status[event.id].rounds[event.round_id][event.flight].scored =
							++status[event.id].rounds[event.round_id][event.flight].scored || 1;

						status[event.id].rounds[event.round_id].in_progress = true;

						statusCache[event.id].pending = ++statusCache[event.id].pending || 1;

					} else if (event.judge_started) {

						status[event.id].rounds[event.round_id][event.flight].started =
							++status[event.id].rounds[event.round_id][event.flight].started || 1;

						status[event.id].rounds[event.round_id].in_progress = true;

						statusCache[event.id].pending = ++statusCache[event.id].pending || 1;

					} else {

						status[event.id].rounds[event.round_id][event.flight].unstarted =
							++status[event.id].rounds[event.round_id][event.flight].unstarted
							|| 1;
					}
				}
			}
		});

		Object.keys(status).forEach( event_id => {

			const sortByRound = arr => {
			   arr.sort((b, a) => {
				   return parseInt(status[event_id].rounds[a].number) - parseInt(status[event_id].rounds[b].number);
			   });
			};

			let rounds = Object.keys(status[event_id].rounds);
			sortByRound(rounds);

			rounds.forEach( round_id => {

				if (statusCache[event_id].pending) { 

					if (!status[event_id].rounds[round_id].in_progress) {
						delete status[event_id].rounds[round_id];
					}
				} else { 

					if (status[event_id].rounds[round_id].unstarted) { 

						if (statusCache[event_id].first_unstarted == 'undefined') {
							statusCache[event_id].first_unstarted = round_id;
						} else { 
							delete status[event_id].rounds[round_id];
						}

					} else { 

						if (statusCache[event_id].first_unstarted) { 
							delete status[event_id].rounds[round_id];
						} else { 

							if (statusCache[event_id].last_done) { 
								delete status[event_id].rounds[statusCache[event_id].last_done];
							}

							statusCache[event_id].last_done = round_id
						}
					}
				}
			});
		});

		const lastQuery = `
			select
				event.id, event.name, event.abbr,
				round.id round_id, round.name round_name, round.label, round.type,
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

			if (status[event.id] == undefined) {
				status[event.id] = {
					abbr   : event.abbr,
					name   : event.name,
					rounds : {}
				};
			}

			if (status[event.id].rounds[event.round_id] == undefined) {
				status[event.id].rounds[event.round_id] = {
					number   : event.round_name,
					flighted : event.flighted,
					type     : event.type,
					label    : event.label,
				}

				if (status[event.id].rounds[event.round_id].label == '') {
					status[event.id].rounds[event.round_id].label = `Rnd ${ event.round_name }`;
				}
			}

			event.round_start = event.round_start || event.timeslot_start;

			if (status[event.id].rounds[event.round_id][event.flight] == undefined) {

				if (status[event.id].rounds[event.round_id][event.flight] == undefined) {
					// this can't seriously be how this works.
					status[event.id].rounds[event.round_id][event.flight] = {};
				}

				status[event.id]
					.rounds[event.round_id][event.flight]
					.start_time
					= new Date(event.round_start).toLocaleTimeString(
						'en-us', {
							hour         : 'numeric',
							minute       : '2-digit',
							timeZone     : event.tz
						});
			}

			if (statusCache[event.id] == undefined) {
				statusCache[event.id] = {};
			}

		});

		if (status.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		}

		return res.status(200).json(status);
	},
}

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
			in		  : 'path',
			name		: 'round_id',
			description : 'Round ID',
			required	: false,
			schema	  : {
				type	: 'integer',
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
