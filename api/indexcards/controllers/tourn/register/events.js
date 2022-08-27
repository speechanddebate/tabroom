export const listEvents = {
	GET: async (req, res) => {
		const db = req.db;
		const tournId = req.params.tourn_id;
		const op = db.Sequelize.Op;

		// Filter out signup options for tournament admins
		const events = await db.event.findAll({
			where: { tourn: tournId },
			include : [
				{ model: db.eventSetting,
					as: 'Settings',
					where : {
						tag: { [op.notLike] : 'signup_%' },
					},
					required: false,
				},
			],
		});

		if (events.count < 1) {
			return res.status(400).json({ message: 'No events found in that tournament' });
		}
		return res.status(200).json(events);

	},
};

export default listEvents;

listEvents.GET.apiDoc = {
	summary: 'Listing of events in the tournament',
	operationId: 'listEvents',
	parameters: [
		{
			in          : 'path',
			name        : 'tourn_id',
			description : 'Tournament ID',
			required    : true,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},
	],
	responses: {
		200: {
			description: 'Event Data',
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
	tags: ['tournament/register'],
};
