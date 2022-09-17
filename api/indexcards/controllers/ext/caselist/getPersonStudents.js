import crypto from 'crypto';
import config from '../../../../config/config';

const getPersonStudents = {
	GET: async (req, res) => {
		const db = req.db;
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		if (req.query.caselist_key !== hash) {
			return res.status(401).json({ message: 'Invalid caselist key' });
		}

		// Get all students on the same roster as the person,
		// but limit to students with future entries with a chapter with future tourns
		// to limit out camp and observer-only chapters
		const students = await db.sequelize.query(`
            SELECT
                DISTINCT S.id,
                S.first,
                S.last,
                CONCAT(S.first, ' ', S.last) AS 'name'
            FROM student S

            INNER JOIN student S2 ON S2.chapter = S.chapter AND S2.id <> S.id
            INNER JOIN entry_student ES ON ES.student = S2.id
            INNER JOIN entry E ON E.id = ES.entry
            INNER JOIN tourn T ON T.id = E.tourn

            INNER JOIN school SC ON SC.chapter = S2.chapter
            INNER JOIN tourn T2 ON T2.id = SC.tourn
            WHERE S2.person = ?
                AND S.retired = 0
                AND T.hidden <> 1
                AND T.start >= CURRENT_TIMESTAMP
                AND T2.hidden <> 1
                AND T2.start >= CURRENT_TIMESTAMP
            GROUP BY S.last, S.first
            ORDER BY S.last, S.first
        `, { replacements: [req.query.person_id] });

		return res.status(200).json([...students[0]]);
	},
};

getPersonStudents.GET.apiDoc = {
	summary: 'Load students for a person ID',
	operationId: 'getPersonStudents',
	parameters: [
		{
			in          : 'query',
			name        : 'person_id',
			description : 'ID of person whose students you wish to access',
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
			description: 'Person Students',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Student' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['caselist'],
};

export default getPersonStudents;
