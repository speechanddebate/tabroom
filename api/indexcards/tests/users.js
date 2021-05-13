
// Some various snippets of data that tests can use to test against.

const now = new Date();

let testPerson = {
	id         : "69",
	email      : 'i.am.test@speechanddebate.org',
	first      : "I",
	middle     : "Am",
	last       : "Test",
	gender     : "O",
	pronoun    : "They/Them",
	no_email   : "0",
	street     : "401 Railroad Pl",
	city       : "West Des Moines",
	state      : "IA",
	zip        : "50265",
	country    : "US",
	tz         : "America/Chicago",
	phone      : "9785551212",
	provider   : "tabroom.com",
	site_admin : false,
	nsda       : "1234",
	password   : '$6$FQy6hwbm$DITMAX.qkTtZYdibOYz4zxqJ94OW82g0qOEnXQeJaRi3t4f4cQl7WwiPISZPSt5vZSgxyeN3ucmFuBAEtoMb0.'
};

export const testPassword = 'fHjFv2CWLxqB';

export const testUser = testPerson;

export const testAdmin = {
	...testPerson,
	id         : "70",
	email      : 'i.am.god@speechanddebate.org',
	last       : "God",
	site_admin : true,
};

export let testUserSession = {
	userkey    : '$6$Pu38ZiKn$FzP6b0eMARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person     : "69",
	ip         : "127.0.0.1",
	created_at : now.toJSON()
};

export let testAdminSession = {
	...testUserSession,
	userkey    : '$6$Pu38ZiKn$FzP6b0eWARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person     : "70",
};

export let testUserTournPerm = {
	tourn  : 1518,
	person : 69,
	tag    : "full_admin"
};

export default {
	testAdmin         : testAdmin,
	testUser          : testUser,
	testUserTournPerm : testUserTournPerm,
	testAdminSession  : testAdminSession,
	testUserSession   : testUserSession,
	testPassword      : testPassword
}

