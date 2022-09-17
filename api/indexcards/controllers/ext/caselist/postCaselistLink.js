import crypto from 'crypto';
import config from '../../../../config/config';

const postCaselistLink = {
	POST: async (req, res) => {
		const db = req.db;
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		if (req.body.caselist_key !== hash) {
			return res.status(401).json({ message: 'Invalid caselist key' });
		}

		await db.sequelize.query(`
            INSERT INTO caselist (slug, eventcode, person)
            VALUES (?, ?, ?)
        `, { replacements: [req.body.slug.trim(), parseInt(req.body.eventcode) || 0, req.body.person_id] });

		return res.status(201).json({ message: 'Successfully created caselist link' });
	},
};

postCaselistLink.POST.apiDoc = {
	summary: 'Create a link to a caselist page',
	operationId: 'postCaselistLink',
	requestBody: {
		description: 'The caselist link',
		required: true,
		content: { '*/*': { schema: { $ref: '#/components/schemas/CaselistLink' } } },
	},
	responses: {
		200: {
			description: 'Caselist Link',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/CaselistLink' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['caselist'],
};

export default postCaselistLink;
