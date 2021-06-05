
import {showDateTime} from '../../../helpers/common'

export const attendance = {

    GET: async (req, res) => {

		const db = req.db;
		const op = db.Sequelize.Op

		let queryLimit = "";

		if (req.params.timeslot_id) {
			req.params.timeslot_id = parseInt(req.params.timeslot_id);
		}

		if (req.params.round_id) {
			req.params.round_id = parseInt(req.params.round_id);
		}

		if (req.params.timeslot_id) {
			queryLimit = " where round.timeslot = "+req.params.timeslot_id;
		} else if (req.params.round_id) {
			queryLimit = " where round.id = "+req.params.round_id;
		} else {
			return res.status(400).json({ message: 'No parameters sent for query' });
		}

        const attendanceQuery = `
			select
				cl.panel panel, cl.tag tag, cl.description description,
					CONVERT_TZ(cl.timestamp, "+00:00", tourn.tz) timestamp,
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

		const startsQuery = `
			select
				judge.person person, panel.id panel,
				ballot.judge_started startTime,
				ballot.audit audited,
				started_by.first startFirst, started_by.last startLast,
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
		`;

        // A raw query to go through the category filter
        const [attendanceResults, attendanceMeta] = await db.sequelize.query(attendanceQuery);
        const [startsResults, startsMeta] = await db.sequelize.query(startsQuery);

		let status = {};

		for (let attend of attendanceResults) {
			status[attend.person] = {
				[attend.panel] : {
					tag         : attend.tag,
					timestamp   : attend.timestamp.toJSON,
					description : attend.description,
				}
			};
		}

		for (let start of startsResults) {

			if (!status[start.person]) {
				status[start.person] = {};
			}

			if (!status[start.person][start.panel]) {
				status[start.person][start.panel] = {};
			}

			status[start.person][start.panel].started_by = start.startFirst+" "+start.startLast;
			status[start.person][start.panel].started = showDateTime(
				start.startTime,
				{ tz: start.tz, format: "daytime" }
			);

			status[start.person][start.panel].audited = start.audited;
		}

		if (status.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		} else {
			return res.status(200).json(status);
		}
    },

    POST: async (req, res) => {

		const {body} = req;
		const now = Date();
		const db = req.db;

		const target = await ( () => {
			try {
				return db.person.findByPk(req.body.target_id);
			} catch(err) {
				return res.status(201).json({
					error   : true,
					message : `Bad parameters sent ${err}`
				});
			}
		})();

		if (!target) {
			return res.status(201).json({
				error   : true,
				message : `No person to mark present for ID ${req.body.target_id}`
			});
		}

		const panel = await ( () => {
			try {
				return db.panel.findByPk(req.body.related_thing);
			} catch(err) {
				return res.status(201).json({
					error   : true,
					message : `Bad parameters sent ${err}`
				});
			}
		})();


		if (!panel) {
			return res.status(201).json({
				error   : true,
				message : `No section found for ID ${req.body.related_thing}`
			});
		}

		if (req.body.setting_name == "judge_started") {

			const judge = await ( () => {
				try {
					return db.judge.findByPk(req.body.another_thing);
				} catch(err) {
					return res.status(201).json({
						error   : true,
						message : `No judge found for ID ${req.body.another_thing}`
					});
				}
			})();

			if (req.body.property_name > 0) {

				const eraseStart = `update ballot set started_by = NULL,
					judge_started = NULL
					where judge = :judgeId
					and panel = :panelId `;

				await db.sequelize.query(eraseStart, {
					replacements: { judgeId: judge.id, panelId: panel.id }
				});

				let response = {
					error : false,
					reclass: [
						{   id          : `${panel.id}_${target.id}_start`,
							removeClass : "greentext",
							addClass    : "yellowtext"
						},{
							id          : `${panel.id}_${target.id}_start`,
							removeClass : "fa-star",
							addClass    : "fa-stop"
						},
					],
					reprop: [
						{   id          : `start_${panel.id}_${target.id}`,
							property	: "property_name",
							value 		: false
						},{
							id          : `start_${panel.id}_${target.id}`,
							property	: "title",
							value 		: "Not started"
						},
					],
					message : "Judge marked as not started"
				};

				return res.status(201).json(response);

			} else {


				try {
					await db.ballot.update({
						started_by: req.session.person,
						judge_started : now
					},{ where : {
						panel : panel.id,
						judge : judge.id
					}});
				} catch(err) {
					return res.status(201).json({
						error   : true,
						message : `Bad parameters sent ${err}`
					});
				}

				let response = {
					error : false,
					reclass: [
						{   id          : `${panel.id}_${target.id}_start`,
							addClass    : "greentext",
							removeClass : "yellowtext"
						},{
							id          : `${panel.id}_${target.id}_start`,
							addClass    : "fa-star",
							removeClass : "fa-stop"
						},
					],
					reprop: [
						{   id          : `start_${panel.id}_${target.id}`,
							property	: "property_name",
							value 		: 1
						},{
							id          : `start_${panel.id}_${target.id}`,
							property	: "title",
							value 		: "Judge marked as started by "+req.session.name,
						},
					],
					message : "Judge marked as started by "+req.session.name,
				};

				return res.status(201).json(response);
			}

		} else if (req.body.property_name == 1) {

			// The property already being 1 means that they're currently present,
			// so mark them as absent.

			const logMessage = `${target.first} ${target.last} marked as absent by ${req.session.email}`;

			let log = {
				tag         : "absent",
				description : logMessage,
				person      : req.body.target_id,
				tourn       : req.params.tourn_id,
				panel       : panel.id
			};

			if (req.body.setting_name == "entry") {
				log.entry = req.body.another_thing;
			} else if (req.body.setting_name == "judge") {
				log.judge = req.body.another_thing;
			}

			try {
				await db.campusLog.create(log);
			} catch(err) {
				return res.status(201).json({
					error   : true,
					message : `Bad parameters sent ${err}`
				});
			}

			// Oh for the days I have react going and don't need to do the
			// following nonsense

			return res.status(201).json({
				error   : false,
				message : logMessage,
				reclass : [
					{	id          : `${panel.id}_${req.body.target_id}`,
						removeClass : "greentext",
						addClass    : "brightredtext"
					},
					{	id          : `${panel.id}_${req.body.target_id}`,
						removeClass : "fa-check",
						addClass    : "fa-circle"
					}
				],
				reprop  : [
					{	id       : `container_${panel.id}_${req.body.target_id}`,
						property : "property_name",
						value    : false
					}
				]
			});

		} else {

			// In this case they're currently marked absent, so we mark them
			// present

			const logMessage = `${target.first} ${target.last} marked as present by ${req.session.email}`;

			let log = {
				tag         : "present",
				description : logMessage,
				person      : req.body.target_id,
				tourn       : req.params.tourn_id,
				panel       : panel.id
			};

			if (req.body.setting_name == "entry") {
				log.entry = req.body.another_thing;
			} else if (req.body.setting_name == "judge") {
				log.judge = req.body.another_thing;
			}

			try {
				await db.campusLog.create(log);
			} catch(err) {
				return res.status(201).json({
					error   : true,
					message : `Bad parameters sent ${err}`
				});
			}

			return res.status(201).json({
				error   : false,
				message : logMessage,
				reclass : [
					{	id          : `${panel.id}_${req.body.target_id}`,
						addClass    : "greentext",
						removeClass : "brightredtext"
					},
					{	id          : `${panel.id}_${req.body.target_id}`,
						addClass    : "fa-check",
						removeClass : "fa-circle"
					}
				],
				reprop  : [
					{	id       : `container_${panel.id}_${req.body.target_id}`,
						property : "property_name",
						value    : 1
					}
				]
			});
		}

	}
};

export default attendance;

attendance.GET.apiDoc = {
    summary: 'Room attedance and start status of a round or timeslot',
    operationId: 'roundStatus',
    parameters: [
        {
            in          : 'path',
            name        : 'tourn_id',
            description : 'Tournament ID',
            required    : false,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },{
            in          : 'path',
            name        : 'round_id',
            description : 'Round ID',
            required    : false,
            schema      : {
				type    : 'integer',
				minimum : 1
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

