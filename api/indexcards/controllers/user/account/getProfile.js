const getProfile = {
	GET: async (req, res) => {

		if (!req.session) {
			return res.status(201).json({ message: 'You have no active user session' });
		}
		const db = req.db;

		let result;

		if (req.params.person_id && req.session.site_admin) {
			result = await db.person.findByPk(
				req.params.person_id,
				{
					include: [{
						model: db.personSetting,
						as: 'Settings',
					}],
				}
			);

		} else if (req.params.person_id ) {
			return res.status(201).json({ message: 'Only admin staff may access another profile' });
		} else if (req.session.person) {
			result = await db.person.findByPk(req.session.person,
				{
					include: [{
						model: db.personSetting,
						as: 'Settings',
					}],
				},
			);
		}

		if (result.count < 1) {
			return res.status(400).json({ message: 'User does not exist' });
		}

		const jsonOutput = result.toJSON();
		delete jsonOutput.password;

		return res.status(200).json(jsonOutput);
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
