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
		const res = await request(server)
			.post(`/v1/caselist/link`)
            .body({ person_id: 17145, slug: '/test', caselist_key: hash })
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(201);
	});

	after('Remove Dummy Data', async () => {
        await db.sequelize.query(`
            DELETE FROM person_setting WHERE person = 17145 AND tag = 'caselist_link'
        `);
		await testAdminSession.destroy();
		await testAdmin.destroy();
	});
});
