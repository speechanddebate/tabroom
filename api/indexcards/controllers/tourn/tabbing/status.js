
export const roundStatus = {
    GET: async (req, res) => {
		const db = req.db;
		const tournId = req.params.tourn_id;
		const op = db.Sequelize.Op

		// Filter out signup options for tournament admins

		let events = await db.event.findAll({
			where: { tourn: tournId },
			include : [
				{ model: db.eventSetting, as: 'Settings',
					where : {
						tag: { [op.notLike] : "signup_%"}
					},
					required: false
				}
			]
		});

		if (events.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		} else {
			return res.status(200).json(events);
		}
    },
};

export const timeslotStatus = {
    GET: async (req, res) => {
		const db = req.db;
		const tournId = req.params.tourn_id;
		const op = db.Sequelize.Op

		// Filter out signup options for tournament admins

		let events = await db.event.findAll({
			where: { tourn: tournId },
			include : [
				{ model: db.eventSetting, as: 'Settings',
					where : {
						tag: { [op.notLike] : "signup_%"}
					},
					required: false
				}
			]
		});

		if (events.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		} else {
			return res.status(200).json(events);
		}
    },
};

roundStatus.GET.apiDoc = {
    summary: 'Room attedance and start status of a round',
    operationId: 'roundStatus',
    parameters: [
        {
            in          : 'path',
            name        : 'tourn_id',
            description : 'Tournament ID',
            required    : true,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },{
            in          : 'path',
            name        : 'round_id',
            description : 'Round ID',
            required    : true,
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
                        type: 'array',
                        items: { $ref: '#/components/schemas/Event' },
                    },
                },
            },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['tourn/tabbing'],
};

timeslotStatus.GET.apiDoc = {
    summary: 'Room attedance and start status of a timeslot',
    operationId: 'timeslotStatus',
    parameters: [
        {
            in          : 'path',
            name        : 'tourn_id',
            description : 'Tournament ID',
            required    : true,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },{
            in          : 'path',
            name        : 'timeslot_id',
            description : 'Timeslot ID',
            required    : true,
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
                        type: 'array',
                        items: { $ref: '#/components/schemas/Event' },
                    },
                },
            },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['tourn/tabbing'],
};
