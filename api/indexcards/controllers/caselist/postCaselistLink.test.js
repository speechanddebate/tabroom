import crypto from 'crypto';
import request from 'supertest';
import config from '../../../config/config.js';
import server from '../../../app.js';
import { testAdminSession } from '../../../tests/testFixtures.js';

describe('Caselist Link', () => {
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
});
