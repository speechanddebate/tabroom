import config from '../../../../config/config'
import db from '../../../models';
import request from 'supertest';
import server from '../../../../app';

import { attendance }  from './status';
import { assert } from 'chai';
import userData from '../../../tests/users';

describe('Status Board', () => {

    let testAdmin = {};
    let testCampusLog = {};
    let testAdminSession = {};

    before("Set Dummy Data", async () => {
		testAdmin = await db.person.create(userData.testAdmin);
		testAdminSession = await db.session.create(userData.testAdminSession);
		testCampusLog = await db.campusLog.create(userData.testCampusLog);
    });

	it('Return a correct JSON status object', async () => {

		const res = await request(server)
			.get(`/v1/tourn/1/tab/status/round/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.expect('Content-Type', /json/)
			.expect(200);

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

	it('Mark an absent as present, and a present as absent', async () => {

	});

	it ("Reflects those changes in a new status object", async() => { 
		let res = await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.body({
				target_id: 16,   	// person marked present
				related_thing: 27, 	// panel ID
			})
			.expect('Content-Type', /json/)
			.expect(200);

		let res = await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.body({target_id: 16, related_thing: 27})
			.expect('Content-Type', /json/)
			.expect(200);

		
	});


    after("Remove Dummy Data", async () => {
        await testCampusLog.destroy();
        await testAdminSession.destroy();
        await testAdmin.destroy();
	});

});

