import crypto from 'crypto';
import config from '../../../../config/config';

const getPersonChapters = {
	GET: async (req, res) => {
		const db = req.db;
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		if (req.query.caselist_key !== hash) {
			return res.status(401).json({ message: 'Invalid caselist key' });
		}
		const student = await db.sequelize.query(`
			SELECT DISTINCT C.id, C.name, C.state
			FROM chapter C
			INNER JOIN student S ON S.chapter = C.id
			WHERE S.person = ?
		`, { replacements: [req.query.person_id] });
		const advisor = await db.sequelize.query(`
			SELECT DISTINCT C.id, C.name, C.state
			FROM chapter C
			INNER JOIN permission P ON P.chapter = C.id AND tag = 'chapter'
			WHERE P.person = ?
		`, { replacements: [req.query.person_id] });

		return res.status(200).json([...student[0], ...advisor[0]]);
	},
};

getPersonChapters.GET.apiDoc = {
	summary: 'Load chapters for a person ID',
	operationId: 'getPersonChapters',
	parameters: [
		{
			in          : 'query',
			name        : 'person_id',
			description : 'ID of person whose chapters you wish to access',
			required    : true,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},
		{
			in          : 'query',
			name        : 'caselist_key',
			description : 'Key for caselist API access',
			required    : true,
			schema      : {
				type    : 'string',
			},
		},
	],
	responses: {
		200: {
			description: 'Person Chapters',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Chapter' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['caselist'],
};

export default getPersonChapters;
