import schemas from './definitions/schemas';
import responses from './definitions/responses';

const apiDoc = {
	openapi: '3.0.2',
	servers: [{ url: '/' }],
	info: {
		title: 'Tabroom IndexCards API v1',
		version: '1.0.0',
	},
	components: {
		schemas,
		responses,
		securitySchemes: { basic: { type: 'http', scheme: 'basic' } },
	},
	paths: {},
	security: [{ basic: [] }],
};

export default apiDoc;
