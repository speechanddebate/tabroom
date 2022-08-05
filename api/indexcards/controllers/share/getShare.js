import db from '../../models/index.cjs';
import { startOfYear } from '../../helpers/common.js';

const getShare = {
	GET: async (req, res) => {
		const panels = await db.sequelize.query(`
			SELECT
				DISTINCT P.id,
				PS.value AS 'share',
				T.name AS 'tournament',
				COALESCE(NULLIF(R.label, ''), NULLIF(R.name, ''), NULLIF(R.type, ''), 'X') AS 'round',
				CASE WHEN B.side = 1 THEN 'Aff' ELSE 'Neg' END AS 'side'
			FROM
				panel P
				INNER JOIN panel_setting PS ON PS.panel = P.id
					AND PS.tag = 'share'
				INNER JOIN ballot B ON B.panel = P.id
				INNER JOIN round R ON R.id = P.round
				INNER JOIN event E ON R.event = E.id
				INNER JOIN tourn T ON T.id = E.tourn
				INNER JOIN entry EN ON EN.id = B.entry
				INNER JOIN entry_student ES ON ES.entry = EN.id
				INNER JOIN student S ON S.id = ES.student
				INNER JOIN person PN ON PN.id = S.person
			WHERE
				PN.id = ?
				AND R.published = 1
				AND E.type = 'debate'
				AND T.start > '${startOfYear}-08-01 00:00:00'
				AND T.start < CURRENT_TIMESTAMP
				AND T.hidden <> 1
			GROUP BY P.id
			`, { replacements: [req.personId] });

		return res.status(200).json(panels[0]);
	},
};

getShare.GET.apiDoc = {
	summary: 'Load panels for a person',
	operationId: 'getShare',
	responses: {
		200: {
			description: 'Person Panels',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Panel' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['caselist'],
};

export default getShare;
