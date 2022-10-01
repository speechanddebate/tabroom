import { assert } from 'chai';
import db from './db';

describe('Data Access Helpers', () => {

	let testTourn = {};

	const testJson = {
		test1: 1,
		test2: 2,
		test3: 'wrongonpurpose',
		test4: false,
		test5: true,
	};

	const testDate =  new Date();

	beforeAll(async () => {
		testTourn = await db.summon(db.tourn, 1);
		await db.sequelize.query(`delete from tourn_setting where tourn = 1 and tag like "test_%"`);
	});

	it('Tournament, I summon thee and all thy settings!', async () => {
		assert.typeOf(testTourn, 'object');
		assert.equal(testTourn.table, 'tourn');
		assert.equal(testTourn.id, 1);
	});

	it('Creates settings for the test tournament', async () => {
		const stringReply = await db.setting(testTourn, 'test_string', 'womp');
		const textReply = await db.setting(testTourn, 'test_text', { text: 'this is something longer that would not fit in a normal varchar!' });
		const jsonReply = await db.setting(testTourn, 'test_json', { json: testJson });
		const dateReply = await db.setting(testTourn, 'test_date', { date: testDate });

		assert.equal(stringReply, 'womp');
		assert.equal(textReply, 'this is something longer that would not fit in a normal varchar!');
		assert.typeOf(jsonReply, 'object');
		assert.equal(jsonReply.test2, 2);
		assert.typeOf(dateReply, 'date');
		assert.equal(dateReply.getMonth(), testDate.getMonth());

	});

	it('Changes settings for the test tournament', async () => {
		const stringReply = await db.setting(testTourn, 'test_string', 'fleebles!');
		const textReply = await db.setting(testTourn, 'test_text', { text: 'lorem ipsum' });

		testJson.test3 = 'correct';
		const jsonReply = await db.setting(testTourn, 'test_json', { json: testJson });

		testDate.setMonth(testDate.getMonth() - 1);
		const dateReply = await db.setting(testTourn, 'test_date', { date: testDate });

		assert.equal(stringReply, 'fleebles!');
		assert.equal(textReply, 'lorem ipsum');
		assert.typeOf(jsonReply, 'object');
		assert.equal(jsonReply.test3, 'correct');
		assert.typeOf(dateReply, 'date');
		assert.equal(dateReply.getMonth(), testDate.getMonth());

	});

	it('Retrieves settings for the test tournament', async () => {
		const stringReply = await db.setting(testTourn, 'test_string');
		const textReply = await db.setting(testTourn, 'test_text');
		const jsonReply = await db.setting(testTourn, 'test_json');
		const dateReply = await db.setting(testTourn, 'test_date');

		assert.equal(stringReply, 'fleebles!');
		assert.equal(textReply, 'lorem ipsum');
		assert.typeOf(jsonReply, 'object');
		assert.equal(jsonReply.test3, 'correct');
		assert.typeOf(dateReply, 'date');
		assert.equal(dateReply.getMonth(), testDate.getMonth());
	});

	it('Deletes settings for the test tournament', async () => {

		let stringReply = await db.setting(testTourn, 'test_string', '');
		let textReply = await db.setting(testTourn, 'test_text', 0);
		let jsonReply = await db.setting(testTourn, 'test_json', null);
		let dateReply = await db.setting(testTourn, 'test_date', 0);

		stringReply = await db.setting(testTourn, 'test_string');
		textReply = await db.setting(testTourn, 'test_text');
		jsonReply = await db.setting(testTourn, 'test_json');
		dateReply = await db.setting(testTourn, 'test_date');

		assert.notExists(stringReply);
		assert.notExists(textReply);
		assert.notExists(jsonReply);
		assert.notExists(dateReply);
	});
});
