import crypto from 'crypto';
import request from 'supertest';
import { assert } from 'chai';
import config from '../../../../config/config';
import server from '../../../../app';
import { testAdminSession } from '../../../../tests/testFixtures';

describe('Person History', () => {
	it('Returns history for a person', async () => {
		const hash = crypto.createHash('sha256').update(config.NSDA_KEY).digest('hex');
		const res = await request(server)
			.get(`/v1/nsda/history?person_id=10288905&nsda_key=${hash}`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.isObject(res.body, 'Response is an object');
		assert.property(res.body, 'personId', 'Response has personId property');
		assert.property(res.body, 'student', 'Response has student property');
		assert.property(res.body, 'judge', 'Response has judge property');
		assert.property(res.body, 'quizzes', 'Response has quizzes property');
	}, 30000);

	it('Errors on a missing person id', async () => {
		const hash = crypto.createHash('sha256').update(config.NSDA_KEY).digest('hex');
		await request(server)
			.get(`/v1/nsda/history?person_id=999999999&nsda_key=${hash}`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(400);
	}, 30000);
});
