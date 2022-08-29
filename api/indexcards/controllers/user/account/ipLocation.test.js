import request from 'supertest';
import { assert } from 'chai';
import server from '../../../../app';

describe('IP Location Results', () => {
	it('Returns correct location data', async () => {
		const res = await request(server)
			.get(`/v1/user/iplocation/1.1.1.1`)
			.set('Accept', 'application/json')
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body.country,
			'Australia',
			'Location is in Oz as it should be'
		);

		assert.isFalse(
			res.body.isEU,
			'Australia may be in Eurovision but it is not in the EU.  But God if it joined? That would SERIOUSLY piss off the Brits.  For it'
		);
	});
});
