import { assert } from 'chai';
import uuid from 'uuid/v4';
import config from '../../config/config';
import db from '../models';
import login from './login';

import userData from '../tests/users';

describe('Login Password Validation', () => {

	let testUser = {};
	let testUserSession = {};

	before('Set Dummy Data', async () => {
		testUser = await db.person.create(userData.testUser);
		testUserSession = await db.session.create(userData.testUserSession);
	});

	it('Authenticates the password correctly for a user', async () => {
		const req = {
			db,
			config,
			uuid     : uuid(),
			params   : {
				email : userData.testUser.email,
				password : userData.testPassword,
			},
		};

		const session = await (login(req));

		assert.typeOf(session, 'object');
		assert.equal(session.person, '69');
		testUser.session = session;

	});

	it('Rejects incorrect login for a user', async () => {
		const req = {
			db,
			config,
			uuid     : uuid(),
			params   : {
				email : userData.testUser.email,
				password : `${userData.testPassword}garbage`,
			},
		};

		const session = await (login(req));
		assert.equal(session, 'Password was incorrect!');
	});
	after('Remove Dummy Data', async () => {
		await testUser.destroy();
		await testUserSession.destroy();
	});
});
