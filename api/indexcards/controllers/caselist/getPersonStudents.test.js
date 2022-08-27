import crypto from 'crypto';
import request from 'supertest';
import { assert } from 'chai';
import config from '../../../config/config.js';
import db from '../../models/index.cjs';
import server from '../../../app.js';
import userData from '../../../tests/users.js';

describe('Person Students', () => {
	let testAdmin = {};
	let testAdminSession = {};

	beforeAll(async () => {
		testAdmin = await db.person.create(userData.testAdmin);
		testAdminSession = await db.session.create(userData.testAdminSession);
	});

	it('Returns students for a person', async () => {
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		const res = await request(server)
			.get(`/v1/caselist/students?person_id=17145&caselist_key=${hash}`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isArray(res.body, 'Response is an array');
	});

	afterAll(async () => {
		await testAdminSession.destroy();
		await testAdmin.destroy();
	});
});
