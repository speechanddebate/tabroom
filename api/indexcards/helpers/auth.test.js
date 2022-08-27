import { assert } from 'chai';
import config from '../../config/config.js';
import db from '../models/index.cjs';
import auth from './auth.js';
import tournAuth from './tourn-auth.js';
import userData from '../tests/users.js';

describe('Authentication Functions', () => {

	let testUser = {};
	let testUserSession = {};
	let testUserTournPerm = {};
	let testAdmin = {};
	let testAdminSession = {};

	before('Set Dummy Data', async () => {
		testUser = await db.person.create(userData.testUser);
		testAdmin = await db.person.create(userData.testAdmin);
		testUserSession = await db.session.create(userData.testUserSession);

		try {
			testAdminSession = await db.session.create(userData.testAdminSession);
		} catch(err) {
			console.log(err);
		}
		testUserTournPerm = await db.permission.create(userData.testUserTournPerm);
	});

	it('Ignores the database if there is already a session', async () => {

		const req = {
			db,
			config,
			session : {
				id  : 69,
			},
		};

		const session = await (auth(req));
		assert.typeOf(session, 'object');
		assert.equal(session.id, '69');
	});

	it('Finds a session for an ordinary user', async () => {

		const req = {
			db,
			config,
			cookies : {
				[config.COOKIE_NAME]: userData.testUserSession.userkey,
			},
		};

		const session = await (auth(req));

		assert.typeOf(session, 'object');
		assert.equal(session.person, '69');
		assert.equal(session.site_admin, false);
		assert.equal(session.email, 'i.am.test@speechanddebate.org');
	});

	it('Permits an ordinary user access to a tournament it is admin for', async () => {

		const testTourn = userData.testUserTournPerm.tourn;

		const req = {
			db,
			config,
			params: {
				tourn_id : testTourn,
			},
			cookies : {
				[config.COOKIE_NAME]: userData.testUserSession.userkey,
			},
		};

		req.session = await (auth(req));
		req.session = await (tournAuth(req));

		assert.typeOf(req.session, 'object');
		assert.equal(req.session[testTourn].level, 'tabber');
		assert.equal(req.session[testTourn].menu, 'all');
	});

	it('Denies user access to a tournament it is not admin for', async () => {

		const testNotTourn = '9700';

		const req = {
			db,
			config,
			params: {
				tourn_id : testNotTourn,
			},
			cookies : {
				[config.COOKIE_NAME]: userData.testUserSession.userkey,
			},
		};

		req.session = await (auth(req));
		req.session = await (tournAuth(req));

		assert.typeOf(req.session, 'object');
		assert.isEmpty(req.session[testNotTourn]);
	});

	it('Finds a session for an GLP Admin user', async () => {
		const req = {
			db,
			config,
			cookies : {
				[config.COOKIE_NAME]: userData.testAdminSession.userkey,
			},
		};

		const session = await (auth(req));

		assert.typeOf(session, 'object');
		assert.equal(session.person, '70');
		assert.equal(session.site_admin, true);
		assert.equal(session.email, 'i.am.god@speechanddebate.org');
	});

	it('Permits GLP admin access to a tournament it is not admin for', async () => {

		const testNotTourn = '9700';

		const req = {
			db,
			config,
			params: {
				tourn_id : testNotTourn,
			},
			cookies : {
				[config.COOKIE_NAME]: userData.testAdminSession.userkey,
			},
		};

		req.session = await (auth(req));
		req.session = await (tournAuth(req));

		assert.typeOf(req.session, 'object');
		assert.equal(req.session[testNotTourn].level, 'owner');
		assert.equal(req.session[testNotTourn].menu, 'all');
	});

	after('Remove Dummy Data', async () => {
		await testUser.destroy();
		await testAdmin.destroy();
		await testUserSession.destroy();
		await testAdminSession.destroy();
		await testUserTournPerm.destroy();
	});
});
