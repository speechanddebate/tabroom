import { assert } from 'chai';
import request from 'supertest';
import server from '../../../../app';

describe('Tournament Front Listing', () => {

	// I haven't loaded up the sample data thing yet so for now I just want to
	// verify that I get SOME valid tournament from this API

	it('Lists upcoming tournaments', async () => {

		const res = await request(server)
			.get(`/v1/invite/upcoming`)
			.set('Accept', 'application/json')
			.expect('Content-Type', /json/)
			.expect(200);

		assert.typeOf(res.body, 'array', 'Array returned');
		assert.typeOf(res.body[0], 'object', 'Array contains objects');

		assert.typeOf(res.body[0].id, 'number', 'Object contains valid ID number');
		assert.typeOf(res.body[0].year, 'number', 'Object contains valid year');
		assert.typeOf(res.body[0].week, 'number', 'Object contains valid week number');
		assert.typeOf(res.body[0].name, 'string', 'Object contains valid names');
	});
});
