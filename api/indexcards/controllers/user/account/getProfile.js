import db from '../../../models';

const getProfile = {
	GET: async (req, res) => {
		const result = await db.Person.findByPk(req.query.userId);

		if (result.count < 1) {
			return res.status(400).json({ message: 'User does not exist' });
		}
		return res.status(200).json(result);
	},
};

getProfile.GET.apiDoc = {
	summary: 'Load the profile data of the logged in user',
	operationId: 'getProfile',
	parameters: [
		{
			in          : 'path',
			name        : 'person_id',
			description : 'ID of user whose profile you wish to access',
			required    : true,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},
	],
	responses: {
		200: {
			description: 'Person Profile',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Person' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['accounts'],
};

export default getProfile;
