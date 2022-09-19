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
	nsda       : '5678',
	site_admin : true,
};

export const testUserSession = {
	id         : '69',
	userkey    : '$6$Pu38ZiKn$FzP6b0eMARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person     : '69',
	ip         : '127.0.0.1',
	created_at : now.toJSON(),
};

export const testAdminSession = {
	...testUserSession,
	id      : '70',
	userkey : '$6$Pu38ZiKn$FzP6b0eWARklPxm9WicHlNQts7tPJNsogbGWLZSQayUZlFBJY29XRDXA1XznWi3/72kej6gp8W0epnpsJLMKP.',
	person  : '70',
};

export const testUserTournPerm = {
	id     : 1,
	tourn  : '1518',
	person : '69',
	tag    : 'tabber',
};

export const testCampusUsers = [
	{ id: 10, email: 'test10@tabroom.com', first: 'Test', last: 'Ten' },
	{ id: 11, email: 'test11@tabroom.com', first: 'Test', last: 'Eleven' },
	{ id: 12, email: 'test12@tabroom.com', first: 'Test', last: 'Twelve' },
	{ id: 13, email: 'test13@tabroom.com', first: 'Test', last: 'Thirteen' },
	{ id: 14, email: 'test14@tabroom.com', first: 'Test', last: 'Fourteen' },
	{ id: 15, email: 'test15@tabroom.com', first: 'Test', last: 'Fifteen' },
	{ id: 16, email: 'test16@tabroom.com', first: 'Test', last: 'Sixteen' },
	{ id: 17, email: 'test17@tabroom.com', first: 'Test', last: 'Seventeen' },
	{ id: 18, email: 'test18@tabroom.com', first: 'Test', last: 'Eighteen' },
];

export const testCampusLogs = [
	{ id : 1       , tag : 'present' , description : 'Test Ten marked as present by palmer@tabroom.com'       , person : 10 , tourn : 1 , panel : 37 },
	{ id : 2852264 , tag : 'present' , description : 'Test Eleven marked as present by palmer@tabroom.com'    , person : 11 , tourn : 1 , panel : 6  , entry : 359864  , timestamp : '2021-05-23 11:04:04' },
	{ id : 2852265 , tag : 'present' , description : 'Test Twelve marked as present by palmer@tabroom.com'    , person : 12 , tourn : 1 , panel : 6  , entry : 3599766 , timestamp : '2021-05-23 11:04:05' },
	{ id : 2852270 , tag : 'present' , description : 'Test Thirteen marked as present by palmer@tabroom.com'  , person : 13 , tourn : 1 , panel : 27 , judge : 354     , timestamp : '2021-05-23 11:04:22' },
	{ id : 2852271 , tag : 'absent'  , description : 'Test Thirteen marked as absent by palmer@tabroom.com'   , person : 13 , tourn : 1 , panel : 27 , judge : 354     , timestamp : '2021-10-26 16:07:26' },
	{ id : 2852272 , tag : 'present' , description : 'Test Fourteen marked as present by palmer@tabroom.com'  , person : 14 , tourn : 1 , panel : 37 , entry : 3599852 , timestamp : '2021-05-23 11:04:41' },
	{ id : 2852273 , tag : 'present' , description : 'Test Fifteen marked as present by palmer@tabroom.com'   , person : 15 , tourn : 1 , panel : 37 , entry : 3599829 , timestamp : '2021-05-23 11:04:42' },
	{ id : 2852267 , tag : 'absent'  , description : 'Test Sixteen marked as absent by palmer@tabroom.com'    , person : 16 , tourn : 1 , panel : 27 , entry : 3599865 , timestamp : '2021-05-23 11:04:18' },
	{ id : 2852268 , tag : 'present' , description : 'Test Seventeen marked as present by palmer@tabroom.com' , person : 17 , tourn : 1 , panel : 27 , entry : 3599781 , timestamp : '2021-05-23 11:04:20' },
	{ id : 2852269 , tag : 'absent'  , description : 'Test Seventeen marked as absent by palmer@tabroom.com'  , person : 17 , tourn : 1 , panel : 27 , entry : 3599781 , timestamp : '2021-05-23 11:04:21' },
];

export const testStoreCartSetting = {
	1: {
		nc      : '20',
		creator : '1',
		tabroom : '10',
		nco     : '30',
		cart_id : '1234567890abcdef',
	},
	2:{
		nc      : '12',
		creator : '1',
		tabroom : '11',
		nco     : '13',
		cart_id : 'abcdef1234567890',
	},
};

export default {
	testAdmin,
	testUser,
	testUserTournPerm,
	testAdminSession,
	testUserSession,
	testPassword,
	testCampusUsers,
	testCampusLogs,
};
