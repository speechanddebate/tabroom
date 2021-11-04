import db from '../../../models/index.cjs';

const getInvite = {

	GET: async (req, res) => {
		if (req.params.webname) {
			const tourn = await db.tourn.findByPk(req.params.webname);
			return res.status(200).json(invite);
		} else if (req.params.tourn_id) {
			const tourn = await db.tourn.findByPk(req.params.webname);
			return res.status(200).json(invite);

		}
	},
};

getInvite.GET.apiDoc = {
	summary: 'Returns the public pages for a tournament',
	operationId: 'getInvite',
	parameters: [
		{
			in: 'path',
			name: 'webname',
			description: 'Public webname of the tournament to return',
			required: false,
			schema: { type: 'string', minimum: 1 },
		},
		{
			in: 'path',
			name: 'tourn_id',
			description: 'Tournament ID of tournament to return',
			required: false,
			schema: { type: 'integer', minimum: 1 },
		},
	],
	responses: {
		200: {
			description: 'Invitationl & General Tournament Info',
			content: { '*/*': { schema: { $ref: '#/components/schemas/Invite' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['invite', 'public'],
};

export default getInvite;
