import request from 'supertest';
import { assert } from 'chai';
import config from '../../../../config/config';
import server from '../../../../app';
import { testAdminSession } from '../../../../tests/testFixtures';

describe('User Profile Loader', () => {
	it('Returns correct JSON for a self profile request', async () => {
		const res = await request(server)
			.get(`/v1/user/profile`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body.email,
			'i.am.god@speechanddebate.org',
			'Correct fake user profile is returned'
		);

		assert.isTrue(res.body.site_admin, 'Site Admin powers are enabled');
	});

	it('Returns correct JSON for another user profile request', async () => {
		const res = await request(server)
			.get(`/v1/user/profile/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body.email,
			'palmer@tabroom.com',
			'Chris Palmer is user number 1'
		);
	});
});
