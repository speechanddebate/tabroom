// Schools at the tournament
export const listSchools = {
	GET: async (req, res) => {
		const db = req.db;
		const tournId = req.params.tourn_id;
		const op = db.Sequelize.Op;

		// Filter out signup options for tournament admins, deliver the rest
		const schools = await db.school.findAll({
			where: { tourn: tournId },
			include : [
				{ model: db.schoolSetting,
					as: 'Settings',
					where : {
						tag: { [op.notLike] : 'signup_%' },
					},
					required: false,
				},
				{ model: db.fine, as: 'Fines' },
				{ model: db.chapter, as: 'Chapter' },
			],
		});

		if (schools.count < 1) {
			return res.status(400).json({ message: 'No schools found in that tournament' });
		}
		return res.status(200).json(schools);
	},
};

export default listSchools;

listSchools.GET.apiDoc = {
	summary: 'Listing of schools in the tournament, with chapter info, settings, and fines',
	operationId: 'listSchools',
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
			description: 'School Data',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/School' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['tournament/register'],
};
