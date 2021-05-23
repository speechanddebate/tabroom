
// Some various snippets of data that tests can use to test against.

const now = new Date();
const later = new Date(now.addDays(3));

// How many of each iterable entry object do you make?
const sampleTarget = 6;

// Offsets to cope with MySQL IDs
const chapterOffset = 40;
const personOffset = 4;
const judgeOffset = 50;
let entryOffset = 7700;

const testPersons = function(personOffset) {

	let persons = [];

	for (let personID = personOffset; personID <= 100; personID++) {
		person.push({
			id     : personID,
			email  : "testuser"+personID+"@tabroom.com",
			first  : "Testy",
			last   : personID,
			gender : "O"
		});
	}
}(personOffset);

const testChapters = function (persons) {

	let studentOffset = 0;
	let chapters = [];

	for (let chapterID = 1; chapterID <= sampleTarget; chapterID++) {

		var chapter = {
			id      : (chapterID + chapterOffset),
			name    : "School "+chapterID,
			level   : "highschool"
			state   : "MA",
		};

		var students = [];

		for (let studentID = 1; studentID <= sampleTarget * 2; studentID++) {

			studentOffset++;

			students.push({
				id        : studentOffset,
				first     : "S" + (chapterID + chapterOffset),
				last      : "Student "+studentOffset,
				grad_year : now.getFullYear(),
				retired   : false,
				novice    : false,
				person    : persons[studentOffset],
				chapter   : (chapterID + chapterOffset)
			});
		}

		chapters.push(chapter);
	}

	return chapters;

}(testPersons);

const testTourn = function (persons, chapters) {

	let tourn = {
		id        : "1",
		name      : "Test Suite Classic",
		city      : "Fitchburg",
		state     : "MA",
		tz        : "America/New_York",
		webname   : "testsuite",
		hidden    : false,
		start     : now,
		end       : new Date(now.addDays(3)),
		reg_start : new Date(now.subtractDays(30)),
		reg_end   : new Date(now.subtractDays(7)),
		Timeslots : [
			{
				id    : "1",
				name  : "Slot 1",
				tourn : "1",
				start : newDate(now.setHours(13).setMinutes(00)),
				end : newDate(now.setHours(14).setMinutes(00)),
			},{
				id    : "2",
				name  : "Slot 2",
				tourn : "1",
				start : newDate(now.setHours(15).setMinutes(00)),
				end : newDate(now.setHours(16).setMinutes(00)),
			},{
				id    : "3",
				name  : "Slot 3",
				tourn : "1",
				start : newDate(now.setHours(17).setMinutes(00)),
				end : newDate(now.setHours(18).setMinutes(00)),
			}, {
				id    : "4",
				name  : "Slot 4",
				tourn : "1",
				start : newDate(now.setHours(19).setMinutes(00)),
				end : newDate(now.setHours(20).setMinutes(00)),
			}
		],
		Categories : [
			{
				id    : "1",
				name  : "Speech",
				abbr  : "IE",
				tourn : "1"
			}, {
				id    : "2",
				name  : "Debate",
				abbr  : "DB",
				tourn : "1"
			}, {
				id    : "3",
				name  : "Congress",
				abbr  : "CON",
				tourn : "1"
			}
		],
		Events: [
			{
				id       : "1",
				name     : "Speech One",
				abbr     : "IE1",
				type     : 'speech',
				level    : 'open',
				fee      : '5.00',
				tourn    : "1",
				category : "1"
			}, {
				id       : "2",
				name     : "Speech Two",
				abbr     : "IE2",
				type     : 'speech',
				level    : 'open',
				fee      : '10.00',
				tourn    : "1",
				category : "1"
			}, {
				id       : "3",
				name     : "Debate One",
				abbr     : "DB1",
				type     : 'debate',
				level    : 'open',
				fee      : '5.00',
				tourn    : "1",
				category : "2"
			}, {
				id       : "4",
				name     : "Debate Two",
				abbr     : "DB2",
				type     : 'debate',
				level    : 'novice',
				fee      : '10.00',
				tourn    : "1",
				category : "2"
			}, {
				id       : "5",
				name     : "Congress",
				abbr     : "CON",
				type     : 'congress',
				level    : 'open',
				fee      : '5.00',
				tourn    : "1",
				category : "3"
			}
		],
		Schools    : [],
		CampusLogs : []
	};

	for (let chapter of chapters) {

		let school = {
			id      : (chapter.id - chapterOffset),
			name    : chapter.name,
			code    : "S"+chapter.id,
			onsite  : true,
			tourn   : "1",
			chapter : chapter.id,
			state   : "MA"
		};

		for (let judgeID = 1; judgeID <= sampleTarget; judgeID++) {
		}

		for (let event of tourn.Events ) {

			for (let entryID = 1; entryID <= sampleTarget; entryID++) {
			}
		}
	}

	for (let event of tourn.Events) {

		let roundNumber = 0;

		for (let timeslot of tourn.Timeslots) {

			roundNumber++;

			let round = {
				event     : event.id,
				timeslot  : timeslot.id,
				name      : roundNumber,
				type      : 'prelim',
				flighted  : 1,
				published : false;
			};

			let sectionTarget = 0;
			let sectionSize = 0;

			// Each event should have 36 entries at this point.
			if (event.type == "speech") {
				sectionTarget =  6;
				sectionSize =  6;
			} else if (event.type == "debate") {
				sectionTarget =  18;
				sectionSize =  2;
			} else if (event.type == "congress") {
				sectionTarget =  2;
				sectionSize =  18;
			}

			for (let sectionLetter = 1; sectionLetter <= sectionTarget; sectionLetter++) {

				let section = {

				}

				for (let ballotTick = 1; ballotTick <= sectionSize; ballotTick++) {

				}
			}
		}
	}

}(testPersons, testChapters);

export default {
	testTourn    : testTourn,
	testChapters : testChapters,
	testPersons  : testPersons
};

