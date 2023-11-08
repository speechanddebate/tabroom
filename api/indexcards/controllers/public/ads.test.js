import { assert } from 'chai';
import request from 'supertest';
import server from '../../../app';

describe('Curent ads', () => {
	it('returns a list of ads', async () => {

		const res = await request(server)
			.get(`/v1/public/ads`)
			.set('Accept', 'application/json')
			.expect('Content-Type', /json/)
			.expect(200);

		assert.typeOf(res.body, 'array', 'Array returned');

		assert.typeOf(res.body[0].id, 'number', 'id of ad is a string');
		assert.typeOf(res.body[0].filename, 'string', 'filename of ad is a string');
		assert.typeOf(res.body[0].url, 'string', 'url of ad is a string');
	});
});
