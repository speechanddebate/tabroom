import crypto from 'crypto';
import request from 'supertest';
import config from '../../../config/config.js';
import db from '../../models/index.cjs';
import server from '../../../app.js';
import userData from '../../tests/users.js';

describe('Caselist Link', () => {
	let testAdmin = {};
	let testAdminSession = {};

	before('Set Dummy Data', async () => {
		testAdmin = await db.person.create(userData.testAdmin);
		testAdminSession = await db.session.create(userData.testAdminSession);
	});

	it('Creates a caselist link', async () => {
		const hash = crypto.createHash('sha256').update(config.CASELIST_KEY).digest('hex');
		await request(server)
			.post(`/v1/caselist/link`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.send({ person_id: 17145, slug: '/test', eventcode: 103, caselist_key: hash })
			.expect('Content-Type', /json/)
			.expect(201);
	});

	after('Remove Dummy Data', async () => {
		await db.sequelize.query(`
            DELETE FROM caselist WHERE person = 17145
        `);
		await testAdminSession.destroy();
		await testAdmin.destroy();
	});
});
