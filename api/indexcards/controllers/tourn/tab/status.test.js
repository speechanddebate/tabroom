import config from '../../../../config/config'
import db from '../../../models';
import request from 'supertest';
import server from '../../../../app';

import { attendance }  from './status';
import { assert } from 'chai';
import userData from '../../../tests/users';

describe('Status Board', () => {

    let testAdmin = {};
    let testAdminSession = {};

    before("Set Dummy Data", async () => {
		testAdmin = await db.person.create(userData.testAdmin);
		testAdminSession = await db.session.create(userData.testAdminSession);
    });

	it('Return a correct JSON status object', async () => {

		const res = await request(server)
			.get(`/v1/tourn/1/tab/status/round/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.expect('Content-Type', /json/)
			.expect(200);

		console.log(res.body);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body[10][37].started_by,
			'Chris Palmer', 'Judge Person 10 marked started by an admin');

		assert.equal(
			res.body[10][37].tag,
			'present', 'Judge Person 10 marked present by an admin');

		assert.equal(
			res.body[11][6].tag,
			'present', 'Entry Person 11 present');

		assert.equal(
			res.body[12][6].tag,
			'present', 'Entry Person 12 present');

		assert.equal(
			res.body[14][37].tag,
			'present', 'Entry Person 14 present');

		assert.equal(
			res.body[15][37].tag,
			'present', 'Entry Person 15 present');

		assert.equal(
			res.body[16][27].tag,
			'absent', 'Entry Person 16 absent');

		assert.equal(
			res.body[17][27].tag,
			'absent', 'Entry Person 17 absent');

		assert.equal(
			res.body[13][27].tag,
			'absent', 'Judge Person 13 absent');

		assert.notProperty(
			res.body[10],
			'6',
			"Judge 10 not present in section 6");

	});

    after("Remove Dummy Data", async () => {
        await testAdminSession.destroy();
        await testAdmin.destroy();
	});

});

