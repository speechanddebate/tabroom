export const systemStatus = {
	GET: async (req, res) => {
		return res.status(200).json({ message: 'OK' });
	},
	POST: async (req, res) => {
		return res.status(200).json({ message: 'OK' });
	},
};

export const barfPlease = {
	GET: async (req, res) => {
		throw new Error('OMG we are not happy');
	},
};

systemStatus.GET.apiDoc = {
	summary: 'Responds with a 200 if up',
	operationId: 'getStatus',
	responses: {
		200: {
			description: 'Server is up',
			content: { '*/*': { schema: { type: 'string' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['systemStatus'],
};

systemStatus.POST.apiDoc = {
	summary: 'Responds with a 200 if up',
	operationId: 'postStatus',
	responses: {
		200: {
			description: 'Server is up',
			content: { '*/*': { schema: { type: 'string' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['systemStatus'],
};

export default systemStatus;
