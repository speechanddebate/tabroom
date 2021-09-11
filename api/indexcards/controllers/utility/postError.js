import { clientLogger } from '../../helpers/logger';

const postError = {
	POST: async (req, res) => {
		await clientLogger.error(req.body.err.message || 'Unspecified client error', req.body);
		return res.status(201).json({ message: 'Error successfully logged' });
	},
	GET: async (req, res) => {
		return res.status(200).json({ message: 'No error created; must use POST' });
	},
};

postError.POST.apiDoc = {
	summary: 'Log an error',
	operationId: 'postError',
	requestBody: {
		description: 'The error data',
		required: true,
		content: { '*/*': { schema: { $ref: '#/components/schemas/LogError' } } },
	},
	responses: {
		201: {
			description: 'Error logged',
			content: { '*/*': { schema: { type: 'string' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	security: [{ basic: [] }],
	'x-tabroom-permissions': {
		user: ['logged_in'],
		error: 'Not authorized to log errors',
	},
	tags: ['errors'],
};

export default postError;
