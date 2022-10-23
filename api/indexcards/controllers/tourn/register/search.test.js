import { assert } from 'chai';
import request from 'supertest';
import server from '../../../../app';
import db from '../../../helpers/db';
import config from '../../../../config/config';
import { testAdminSession }  from '../../../../tests/testFixtures';

describe('Attendee Search Function', () => {

	// Using an old tournament with stable data rather than the test tournament
	// this time because I don't need particular baroque issues

	let adminSession = {};

	beforeAll(async () => {
		adminSession = await db.session.findByPk(testAdminSession.id);
	});

	it('Searches for tournament attendees by name', async () => {

		const searchNorthwestern = 'Northwestern';

		const resNU = await request(server)
			.get(`/v1/tourn/1518/register/search/${searchNorthwestern}`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${adminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.typeOf(resNU.body, 'object', 'Object returned');
		assert.typeOf(resNU.body.exactMatches, 'array', 'Array of exact matches found');
		assert.typeOf(resNU.body.partialMatches, 'array', 'Array of partial matches found');

		assert.typeOf(resNU.body.partialMatches[0].id, 'number', 'ID of partial match is a number');
		assert.typeOf(resNU.body.partialMatches[0].name, 'string', 'Name of partial match is a number');

		assert.typeOf(resNU.body.exactMatches[0].id, 'number', 'ID of exact matches is a number');
		assert.equal(resNU.body.exactMatches[0].id, 42163, 'Exact match ID is correct');
		assert.equal(resNU.body.exactMatches[0].name, 'Northwestern', 'Exact match name is correct');
		assert.equal(resNU.body.exactMatches[0].tag, 'school', 'Exact match tag is correct');

		assert.equal(resNU.body.partialMatches[0].id, 107589, 'Exact match ID is correct');
		assert.equal(resNU.body.partialMatches[0].first, 'Peyton', 'Partial match name is correct');
		assert.equal(resNU.body.partialMatches[0].tag, 'entry', 'Exact match tag is correct');

		// Search for an individual in that same tournament and BONUS ROUND
		// make sure the special character doesn't mess with us

		const searchDaisy = 'O\'Gorman';

		const resDVOG = await request(server)
			.get(`/v1/tourn/1518/register/search/${searchDaisy}`)
			.set('Accept', 'application/json')
			.set('Cookie', [`${config.COOKIE_NAME}=${adminSession.userkey}`])
			.expect('Content-Type', /json/)
			.expect(200);

		assert.typeOf(resDVOG.body, 'object', 'Object returned');
		assert.typeOf(resDVOG.body.exactMatches, 'array', 'Array of exact matches found');
		assert.typeOf(resDVOG.body.partialMatches, 'array', 'Array of partial matches found');
		assert.equal(resDVOG.body.partialMatches.length, 0, 'Array of partial matches is empty');
		assert.equal(resDVOG.body.exactMatches[0].first, 'Danielle', 'Name match found for exact match');
		assert.equal(resDVOG.body.exactMatches[0].tag, 'judge', 'Exact match tag is correct');
	});
});
