import crypto from 'crypto';
import config from '../../../config/config.js';
import db from '../../models/index.cjs';
import { startOfYear } from '../../helpers/common.js';

const getPersonRounds = {
	GET: async (req, res) => {
        const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
        if (req.query.caselist_key !== hash) { 
            return res.status(401).json({ message: 'Invalid caselist key' });
        }
        const persons = await db.sequelize.query(`
            SELECT DISTINCT PS.person
            FROM person_setting PS
            WHERE tag = 'caselist_link'
            AND value_text = ?
        `, { replacements: [req.query.slug] });

        if (!persons || persons[0].length < 1 || !persons[0][0].person) {
            return res.status(404).json({ message: 'No caselist links found' });
        }

        const ids = persons[0].map(p => p.person);

        let rounds = await db.sequelize.query(`
            SELECT
                DISTINCT P.id,
                T.name AS 'tournament',
                COALESCE(NULLIF(R.label, ''), NULLIF(R.name, ''), NULLIF(R.type, ''), 'X') AS 'round',
                CASE WHEN B.side = 1 THEN 'Aff' ELSE 'Neg' END AS 'side',
                O.code AS 'opponent',
                GROUP_CONCAT(DISTINCT J.last) AS 'judge'
            FROM
                panel P
                INNER JOIN ballot B ON B.panel = P.id
                INNER JOIN judge J ON J.id = B.judge
                INNER JOIN round R ON R.id = P.round
                INNER JOIN event E ON R.event = E.id
                INNER JOIN tourn T ON T.id = E.tourn
                INNER JOIN entry EN ON EN.id = B.entry
                INNER JOIN entry_student ES ON ES.entry = EN.id
                INNER JOIN student S ON S.id = ES.student
                INNER JOIN person PN ON PN.id = S.person

                INNER JOIN ballot OB ON OB.panel = P.id AND OB.id <> B.id
                INNER JOIN entry O ON O.id = OB.entry
            WHERE
                PN.id IN (?)
                AND R.published = 1
                AND E.type = 'debate'
                AND T.start > '${startOfYear}-08-01 00:00:00'
                AND T.start < CURRENT_TIMESTAMP
                AND T.hidden <> 1
            GROUP BY P.id
            `, { replacements: [ids.toString()] });

        rounds = rounds[0].filter(r => r.id);

		return res.status(200).json(rounds);
	},
};

getPersonRounds.GET.apiDoc = {
	summary: 'Load rounds for a person ID',
	operationId: 'getPersonRounds',
	parameters: [
		{
			in          : 'query',
			name        : 'slug',
			description : 'Slug of page to match rounds',
			required    : true,
			schema      : {
				type    : 'string',
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
			description: 'Person Rounds',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Round' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['caselist'],
};

export default getPersonRounds;
