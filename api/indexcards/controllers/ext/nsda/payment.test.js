import { assert } from 'chai';
import request from 'supertest';
import config from '../../../../config/config';
import db from '../../../helpers/db';
import server from '../../../../app';
import userData from '../../../../tests/testFixtures';

describe('Status Board', () => {

	let testAdminSession = {};

	beforeAll(async () => {
		testAdminSession = await db.session.findByPk(userData.testAdminSession.id);
	});

	it('Posts a payment into a tournament', async () => {

		const res = await request(server)
			.post(`/v1/nsda/payment`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${testAdminSession.userkey}`])
			.send({
				invoice_id : '10-1',
			})
			.expect('Content-Type', /json/)
			.expect(201);

		assert.equal(
			res.body[10][37].started_by,
			'Chris Palmer',
			'Judge Person 10 marked started by an admin');

		assert.equal(
			res.body[10][37].tag,
			'present',
			'Judge Person 10 marked present by an admin');
	});
});
