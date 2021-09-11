import { assert } from 'chai';
import { academicYear, ordinalize, escapeCSV, emailValidator, condenseDateRange, showDateTime } from './common';

describe('Academic Year helper', () => {
	it('Returns the correct academic year given a Date object', async () => {
		let tick = new Date(2017, 9, 1);
		assert.strictEqual(academicYear(tick), '2017-2018', 'Correct date in fall');
		tick = new Date(2018, 2, 1);
		assert.strictEqual(academicYear(tick), '2017-2018', 'Correct date in spring');
		tick = new Date(2019, 2, 1);
		assert.strictEqual(academicYear(tick), '2018-2019', 'Correct date in following year');
		tick = new Date(2019, 6, 30);
		assert.strictEqual(academicYear(tick), '2018-2019', 'Correct date just under edge');
		tick = new Date(2019, 7, 1);
		assert.strictEqual(academicYear(tick), '2019-2020', 'Correct date just over edge');
	});
});

describe('Ordinalize helper', () => {
	it('Returns ordinalized strings', async () => {
		assert.strictEqual(ordinalize(1), '1st');
		assert.strictEqual(ordinalize(2), '2nd');
		assert.strictEqual(ordinalize(3), '3rd');
		assert.strictEqual(ordinalize(4), '4th');
		assert.strictEqual(ordinalize(5), '5th');
		assert.strictEqual(ordinalize(6), '6th');
		assert.strictEqual(ordinalize(7), '7th');
		assert.strictEqual(ordinalize(8), '8th');
		assert.strictEqual(ordinalize(9), '9th');
		assert.strictEqual(ordinalize(10), '10th');
		assert.strictEqual(ordinalize(11), '11th');
		assert.strictEqual(ordinalize(12), '12th');
		assert.strictEqual(ordinalize(13), '13th');
		assert.strictEqual(ordinalize(14), '14th');
		assert.strictEqual(ordinalize(15), '15th');
		assert.strictEqual(ordinalize(100), '100th');
		assert.strictEqual(ordinalize(101), '101st');
		assert.strictEqual(ordinalize(102), '102nd');
		assert.strictEqual(ordinalize(103), '103rd');
		assert.strictEqual(ordinalize(104), '104th');
	});
});

describe('escapeCSV helper', () => {
	it('Returns a CSV escaped string', async () => {
		assert.strictEqual(escapeCSV('Test, "test"'), '"Test, test",');
		assert.strictEqual(escapeCSV('Test\\'), '"Test",');
		assert.strictEqual(escapeCSV(''), '\\N,');
		assert.strictEqual(escapeCSV(null), '\\N,');
		assert.strictEqual(escapeCSV(undefined), '\\N,');
		assert.strictEqual(escapeCSV(false), '\\N,');

		// Exclude trailing comma
		assert.strictEqual(escapeCSV('test', true), '"test"');
	});
});

describe('emailValidator helper', () => {
	it('Returns email validation', async () => {
		assert.isOk(emailValidator.test('test@test.com'));
		assert.isNotOk(emailValidator.test('test'));
		assert.isNotOk(emailValidator.test('test@'));
		assert.isNotOk(emailValidator.test('test test@test.com'));
	});
});

describe('Condense date range helper', () => {
	it('Returns a single date for the same day', async () => {
		assert.strictEqual(
			condenseDateRange('2017-08-01 08:00:00', '2017-08-01 08:00:00'),
			'8/1/2017',
			'Correct single date'
		);
	});

	it('Returns the range for different days', async () => {
		assert.strictEqual(
			condenseDateRange('2017-08-01 08:00:00', '2017-08-02 08:00:00'),
			'8/1/2017 - 8/2/2017',
			'Correct date range'
		);
	});

	it('Returns nothing for invalid dates', async () => {
		assert.strictEqual(
			condenseDateRange('', ''),
			'',
			'Correct date range'
		);
	});
});

describe('DateTime Formatter', () => {

	const dateSample = '2020-11-07 16:30:00';

	it('Returns a Date object if given a MySQL Date string and no other options', async () => {

		const dtObject = showDateTime(dateSample);
		assert.instanceOf(dtObject, Date, 'Object is a date');
		assert.equal(dtObject.getFullYear(), '2020', 'Year is correct');

	});

	it('Returns a US formatted EDT date given locale & options', async () => {

		const dtString = showDateTime(dateSample, {
			locale : 'en-us',
			tz     : 'America/New_York',
			format : 'long',
		});

		assert.typeOf(dtString, 'string', 'Return a string');

		assert.equal(
			dtString,
			'Sat, November 7, 2020, 11:30 AM EST',
			'String format is correct'
		);
	});

});
