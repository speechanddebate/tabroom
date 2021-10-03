import schemas from './definitions/schemas/index.js';
import responses from './definitions/responses/index.js';

const apiDoc = {
	openapi: '3.0.2',
	servers: [{
		url: '/v1',
	}],
	info: {
		title: 'IndexCards API',
		version: '1.0.0',
		description: 'Tabroom.com data & operational API',
		license: {
			name : 'Copyright 2014-2021, National Speech & Debate Assocation',
		},
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
