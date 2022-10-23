import { assert } from 'chai';
import request from 'supertest';
import server from '../../../app';

describe('Tournament Search Function', () => {

	// A string based search will always be imprecise in terms of numbers of
	// results and all I really care about is whether it works and delivers
	// valid results, so instead of searching against test data here I'm just
	// picking a super generic term and making sure I got back answers.  So, I
	// just search for NCFL Grand Nationals since it will yield both exact and
	// partial matches.

	it('Searches for tournaments by name', async () => {

		const searchParam = 'Grand Nationals';

		const res = await request(server)
			.get(`/v1/public/search/all/${searchParam}`)
			.set('Accept', 'application/json')
			.expect('Content-Type', /json/)
			.expect(200);

		assert.typeOf(res.body, 'object', 'Object returned');
		assert.typeOf(res.body.exactMatches, 'array', 'Array of exact matches found');
		assert.typeOf(res.body.partialMatches, 'array', 'Array of partial matches found');

		assert.typeOf(res.body.partialMatches[0].id, 'number', 'ID of partial match is a number');
		assert.typeOf(res.body.partialMatches[0].name, 'string', 'Name of partial match is a number');
		assert.typeOf(res.body.exactMatches[0].id, 'number', 'Exact match ID is a number');
		assert.equal(res.body.exactMatches[0].webname, 'ncfl', 'Exact match webname clears');
	});
});
