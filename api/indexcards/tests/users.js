// Some various snippets of data that tests can use to test against.
const now = new Date();

const testPerson = {
	id         : '69',
	email      : 'i.am.test@speechanddebate.org',
	first      : 'I',
	middle     : 'Am',
	last       : 'Test',
	gender     : 'O',
	pronoun    : 'They/Them',
	no_email   : '0',
	street     : '401 Railroad Pl',
	city       : 'West Des Moines',
	state      : 'IA',
	zip        : '50265',
	country    : 'US',
	tz         : 'America/Chicago',
	phone      : '9785551212',
	provider   : 'tabroom.com',
	site_admin : false,
	nsda       : '1234',
	password   : '$6$FQy6hwbm$DITMAX.qkTtZYdibOYz4zxqJ94OW82g0qOEnXQeJaRi3t4f4cQl7WwiPISZPSt5vZSgxyeN3ucmFuBAEtoMb0.',
};

export const testPassword = 'fHjFv2CWLxqB';

export const testUser = testPerson;

export const testAdmin = {
	...testPerson,
	id         : '70',
	email      : 'i.am.god@speechanddebate.org',
	last       : 'God',
	site_admin : true,
};

export const testUserSession = {
	userkey    : '$6$Pu38ZiKn$FzP6b0eMARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person     : '69',
	ip         : '127.0.0.1',
	created_at : now.toJSON(),
};

export const testAdminSession = {
	...testUserSession,
	userkey : '$6$Pu38ZiKn$FzP6b0eWARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person  : '70',
};

export const testUserTournPerm = {
	tourn  : 1518,
	person : 69,
	tag    : 'tabber',
};

export const testCampusLog = {
	tag         : 'present',
	person      : 10,
	tourn       : 1,
	description : 'Test Eleven marked as present by palmer@tabroom.com',
	panel       : 37,
};

export default {
	testAdmin,
	testUser,
	testUserTournPerm,
	testAdminSession,
	testUserSession,
	testPassword,
	testCampusLog,
};
