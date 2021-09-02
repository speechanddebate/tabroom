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

		await db.sequelize.query(`
			update ballot 
			set judge_started = NOW(), started_by = 1
			where ballot.judge = 355 
			and ballot.panel = 37
		`);

		await db.sequelize.query(`delete from campus_log where person = 15 and tag = 'absent'`);
		await db.sequelize.query(`delete from campus_log where person = 16 and tag = 'present'`);
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

	it ("Reflects absence & presence changes in a new status object", async() => { 

		// Mark Entry 16 section 27 present
		
		let dummy = await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.send({
				target_id     : 16,   	// person who was absent now present
				related_thing : 27, 	// panel ID
				property_name : 0
			})
			.expect('Content-Type', /json/)
			.expect(201);

		// Mark Entry 15 section 37 absent
		dummy = await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.send({
				target_id     : 15,   	// person who was absent now present
				related_thing : 37, 	// panel ID
				property_name : 1
			})
			.expect('Content-Type', /json/)
			.expect(201);

		// Mark Judge 355 Person 10 Section 37 as not started
		dummy = await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.send({
				target_id     : 10,   	// person who was absent now present
				related_thing : 37, 	// panel ID
				setting_name  : 'judge_started',
				another_thing : 355,	// why yes I do hate this little library I cobbled together
				property_name : 1		// was started, should now be unstarted
			})
			.expect('Content-Type', /json/)
			.expect(201);

		const newResponse = await request(server)
			.get(`/v1/tourn/1/tab/status/round/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [config.COOKIE_NAME+'='+testAdminSession.userkey])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(newResponse.body, 'Response is indeed an object');

		assert.isUndefined(
			newResponse.body[10][37].started_by,
			'After the change, Judge Person 10 not marked started');

		assert.equal(
			newResponse.body[16][27].tag,
			'present', 'After the change, Entry Person 16 present');

		assert.equal(
			newResponse.body[15][37].tag,
			'absent', 'After the change, Entry Person 17 absent');

	});

    after("Remove Dummy Data", async () => {
        await testCampusLog.destroy();
        await testAdminSession.destroy();
        await testAdmin.destroy();
	});

});

