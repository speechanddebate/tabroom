import request from 'supertest';
import { assert } from 'chai';
import config from '../../../../config/config.js';
import db from '../../../models/index.cjs';
import server from '../../../../app.js';
import userData from '../../../tests/users.js';

describe('User Profile Loader', () => {

	let testAdmin = {};
	let testAdminSession = {};

	before('Set Dummy Data', async () => {
		testAdmin = await db.person.create( userData.testAdmin);
		testAdminSession = await db.session.create( userData.testAdminSession);
	});

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

	after('Remove Dummy Data', async () => {
		await testAdminSession.destroy();
		await testAdmin.destroy();
	});

});
