import { assert } from 'chai';
import request from 'supertest';
import hmacSHA512 from 'crypto-js/hmac-sha512';
import Base64 from 'crypto-js/enc-base64';
import config from '../../../../config/config';
import db from '../../../helpers/db';
import server from '../../../../app';
import { testAdminSession, testStoreCartSetting }  from '../../../../tests/testFixtures';

describe('Payment Gateway', () => {

	let adminSession = {};
	let testTourn = {};

	beforeAll(async () => {
		adminSession = await db.session.findByPk(testAdminSession.id);
		testTourn = await db.summon(db.tourn, 1);
		await db.setting(testTourn, 'store_carts', { json: testStoreCartSetting });
	});

	it('Posts a payment into a tournament', async () => {

		const hashDigest = Base64.stringify(hmacSHA512('1234567890abcdef-1', config.NSDA_API_KEY));

		await request(server)
			.post(`/v1/nsda/payment`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${adminSession.userkey}`])
			.send({
				tourn_id   : 1,
				invoice_id : '1234567890abcdef-1',
				hash_key   : hashDigest,
				items      : {
					[config.NSDA_PRODUCT_CODES.tabroom] : 10,
					[config.NSDA_PRODUCT_CODES.nc] : 20,
					[config.NSDA_PRODUCT_CODES.nco] : 30,
				},
			})
			.expect('Content-Type', /json/)
			.expect(201);

		const tourn = await db.summon(db.tourn, 1);

		assert.typeOf(tourn, 'object');
		assert.typeOf(tourn.settings, 'object');
		assert.typeOf(tourn.settings.store_carts, 'object');

		assert.equal(tourn.settings.store_carts[1].tabroom, 10);
		assert.equal(tourn.settings.store_carts[1].nc, 20);
		assert.equal(tourn.settings.store_carts[1].nco, 30);
		assert.equal(tourn.settings.store_carts[1].cart_id, '1234567890abcdef');
	});

	afterAll(async () => {
		await db.setting(testTourn, 'store_carts', 0);
	});
});
