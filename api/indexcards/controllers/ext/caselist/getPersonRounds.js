import crypto from 'crypto';
import { startOfYear } from '@speechanddebate/nsda-js-utils';
import config from '../../../../config/config';

const getPersonRounds = {
	GET: async (req, res) => {
		const db = req.db;
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		if (req.query.caselist_key !== hash) {
			return res.status(401).json({ message: 'Invalid caselist key' });
		}

		if (!req.query.person_id && !req.query.slug) {
			return res.status(400).json({ message: 'One of person_id or slug is required' });
		}

		let ids;

		// If person_id is provided, look up that person's rounds
		// The ID is provided by the caselist after authentication, so this only allows a user to
		// look up their own rounds, not an arbitrary person_id
		if (req.query.person_id) {
			ids = [req.query.person_id];
		} else {
			// If no person_id provided, look up any linked person_id's based on the slug
			// This allows looking up other people's rounds if they've opted in to linking themselves to a page
			const persons = await db.sequelize.query(`
				SELECT DISTINCT C.person
				FROM caselist C
				WHERE slug = ?
			`, { replacements: [req.query.slug] });

			if (!persons || persons[0].length < 1 || !persons[0][0].person) {
				return res.status(404).json({ message: 'No caselist links found' });
			}
			ids = persons[0].map(p => p.person);
		}

		let sql = `
			SELECT
				DISTINCT P.id,
				T.name AS 'tournament',
				COALESCE(NULLIF(R.label, ''), NULLIF(R.name, ''), NULLIF(R.type, ''), 'X') AS 'round',
				CASE WHEN B.side = 1 THEN 'A' ELSE 'N' END AS 'side',
				O.code AS 'opponent',
				GROUP_CONCAT(DISTINCT J.last) AS 'judge',
				R.start_time AS 'start',
				CASE WHEN R.start_time > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 DAY) THEN PS.value ELSE NULL END AS 'share'
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
				LEFT JOIN panel_setting PS ON PS.panel = P.id
					AND PS.tag = 'share'
			WHERE
				PN.id IN (?)
				AND R.published = 1
				AND E.type = 'debate'
				AND T.hidden <> 1
				AND T.start > '${startOfYear}-08-01 00:00:00'
				AND T.start < DATE_ADD(CURRENT_TIMESTAMP, INTERVAL 1 HOUR)
		`;

		if (req.query.current) {
			sql += `
				AND R.start_time > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 6 HOUR)
			`;
		}

		sql += `
			GROUP BY P.id
			ORDER BY R.start_time DESC
		`;

		let rounds = await db.sequelize.query(sql, { replacements: [ids.toString()] });
		rounds = rounds[0].filter(r => r.id);
		rounds.forEach(r => {
			const numeric = parseInt(r.round?.replace(/[^\d]/g, '')?.trim()) || 0;
			if (numeric > 0 && numeric < 10) {
				r.round = numeric.toString();
			} else {
				if (r.round?.toLowerCase()?.includes('quad')) { r.round = 'Quads'; }
				if (r.round?.toLowerCase()?.includes('qd')) { r.round = 'Quads'; }
				if (r.round?.toLowerCase()?.includes('qd')) { r.round = 'Quads'; }
				if (r.round?.toLowerCase()?.includes('128')) { r.round = 'Quads'; }
				if (r.round?.toLowerCase()?.includes('tri')) { r.round = 'Triples'; }
				if (r.round?.toLowerCase()?.includes('trp')) { r.round = 'Triples'; }
				if (r.round?.toLowerCase()?.includes('64')) { r.round = 'Triples'; }
				if (r.round?.toLowerCase()?.includes('dou')) { r.round = 'Doubles'; }
				if (r.round?.toLowerCase()?.includes('dbl')) { r.round = 'Doubles'; }
				if (r.round?.toLowerCase()?.includes('32')) { r.round = 'Doubles'; }
				if (r.round?.toLowerCase()?.includes('oct')) { r.round = 'Octas'; }
				if (r.round?.toLowerCase()?.includes('16')) { r.round = 'Octas'; }
				if (r.round?.toLowerCase()?.includes('quar')) { r.round = 'Quarters'; }
				if (r.round?.toLowerCase()?.includes('qrt')) { r.round = 'Quarters'; }
				if (r.round?.toLowerCase()?.includes('sem')) { r.round = 'Semis'; }
				if (r.round?.toLowerCase()?.includes('fin')) { r.round = 'Finals'; }
			}
		});

		// Remove share link if looking up another person's rounds,
		// share links should only be accessible by the person themselves
		if (!req.query.person_id || req.query.slug) {
			rounds.forEach(r => {
				delete r.share;
			});
		}

		return res.status(200).json(rounds);
	},
};

getPersonRounds.GET.apiDoc = {
	summary: 'Load rounds for a person ID',
	operationId: 'getPersonRounds',
	parameters: [
		{
			in          : 'query',
			name        : 'person_id',
			description : 'Person ID to get rounds for',
			required    : false,
			schema      : {
				type    : 'integer',
			},
		},
		{
			in          : 'query',
			name        : 'slug',
			description : 'Slug of page to match rounds',
			required    : false,
			schema      : {
				type    : 'string',
			},
		},
		{
			in          : 'query',
			name        : 'current',
			description : 'Whether to return only current rounds',
			required    : false,
			schema      : {
				type    : 'boolean',
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
