/* eslint-disable jest/no-disabled-tests */
import request from 'supertest';
import { assert } from 'chai';
import config from '../../../../config/config';
import db from '../../../helpers/db';
import server from '../../../../app';
import userData from '../../../../tests/testFixtures';

describe.skip('Status Board', () => {
	let testAdminSession = {};

	beforeAll(async () => {
		testAdminSession = await db.session.findByPk(userData.testAdminSession.id);
		await db.sequelize.query(`
			update ballot
				set judge_started = NOW(), started_by = 1
			where ballot.judge = 355
				and ballot.panel = 37
		`);
		await db.sequelize.query(`update campus_log set timestamp = NOW() where person = 13 and tag = 'absent'`);
		await db.sequelize.query(`delete from campus_log where person = 15 and tag = 'absent'`);
		await db.sequelize.query(`delete from campus_log where person = 16 and tag = 'present'`);
	});

	it('Return a correct JSON status object', async () => {

		const res = await request(server)
			.get(`/v1/tourn/1/tab/status/round/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body.person[10][37].started_by,
			'Chris Palmer',
			'Judge Person 10 marked started by an admin');

		assert.equal(
			res.body.person[10][37].tag,
			'present',
			'Judge Person 10 marked present by an admin');

		assert.equal(
			res.body.person[11][6].tag,
			'present',
			'Entry Person 11 present');

		assert.equal(
			res.body.person[12][6].tag,
			'present',
			'Entry Person 12 present');

		assert.equal(
			res.body.person[14][37].tag,
			'present',
			'Entry Person 14 present');

		assert.equal(
			res.body.person[15][37].tag,
			'present',
			'Entry Person 15 present');

		assert.equal(
			res.body.person[16][27].tag,
			'absent',
			'Entry Person 16 absent');

		assert.equal(
			res.body.person[17][27].tag,
			'absent',
			'Entry Person 17 absent');

		assert.equal(
			res.body.person[13][27].tag,
			'absent',
			'Judge Person 13 absent');

		assert.notProperty(
			res.body.person[10],
			'6',
			'Judge 10 not present in section 6');
	});

	it('Reflects absence & presence changes in a new status object', async() => {

		// Mark Entry 16 section 27 present
		await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.send({
				target_id     : 16,   	// person who was absent now present
				related_thing : 27, 	// panel ID
				property_name : 0,
			})
			.expect('Content-Type', /json/)
			.expect(201);

		// Mark Entry 15 section 37 absent
		await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.send({
				target_id     : 15,   	// person who was absent now present
				related_thing : 37, 	// panel ID
				property_name : 1,
			})
			.expect('Content-Type', /json/)
			.expect(201);

		// Mark Judge 355 Person 10 Section 37 as not started
		await request(server)
			.post(`/v1/tourn/1/tab/status/update`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.send({
				target_id     : 10,   	// person who was absent now present
				related_thing : 37, 	// panel ID
				setting_name  : 'judge_started',
				another_thing : 355,	// why yes I do hate this little library I cobbled together
				property_name : 1,		// was started, should now be unstarted
			})
			.expect('Content-Type', /json/)
			.expect(201);

		const newResponse = await request(server)
			.get(`/v1/tourn/1/tab/status/round/1`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(newResponse.body, 'Response is indeed an object');

		assert.equal(
			newResponse.body.person[16][27].tag,
			'present',
			'After the change, Entry Person 16 present');

		assert.equal(
			newResponse.body.person[15][37].tag,
			'absent',
			'After the change, Entry Person 17 absent');

		assert.isUndefined(
			newResponse.body.person[10][37].started_by,
			'After the change, Judge Person 10 not marked started');

	});
});

describe.skip('Event Dashboard', () => {
	it('Return a correct JSON status object for the event dashboard', async () => {
		const res = await request(server)
			.get(`/v1/tourn/1/tab/dashboard`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${userData.testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');

		assert.equal(
			res.body[7].abbr,
			'LD',
			'Event 7 is LD');
		assert.equal(
			res.body[7].rounds[1][1].unstarted,
			'25',
			'15 unstarted in Round 1 flight 1');

		assert.isTrue(
			res.body[7].rounds[1][2].undone,
			'Flight 2 is not done');
	});
});
