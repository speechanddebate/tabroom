
export const attendance = {
    GET: async (req, res) => {

		const db = req.db;
		const op = db.Sequelize.Op

		let queryLimit = "";

		if (req.params.timeslot_id && typeof req.params.timeslot_id == "integer") {
			queryLimit = " where round.timeslot = ".req.params.timeslot_id;
		if (req.params.round_id && typeof req.params.round_id == "integer") {
			queryLimit = " where round.id = ".req.params.round_id;
		} else {
			return;
		}

        const attendanceQuery = `
			select
				cl.panel, cl.tag, cl.description,
					CONVERT_TZ(cl.timestamp, '+00:00', tourn.tz),
				person.id, person.first, person.last

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

		const startQuery = `
			select
				ballot.id,  judge.person, panel.id,
				CONVERT_TZ(ballot.judge_started, '+00:00', tourn.tz),
				started_by.first, started_by.last

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
        var results = await db.sequelize.query(
            judgeQuery,
            { replacements: {tourn: tournId }}
        );



		if (events.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		} else {
			return res.status(200).json(events);
		}
    },
};

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

