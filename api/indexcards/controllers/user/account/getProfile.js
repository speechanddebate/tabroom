import db from '../../../models/index.cjs';

const getProfile = {
	GET: async (req, res) => {

		let result;

		if (req.params.person_id && req.session.site_admin) {
			result = await db.person.findByPk(req.params.person_id);
		} else if (req.params.person_id ) {
			return res.status(201).json({ message: 'Only admin staff may access another profile' });
		} else {
			result = await db.person.findByPk(req.session.person);
		}

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
			description : 'ID of user whose profile you wish to access.  Defaults to present session.',
			required    : false,
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
