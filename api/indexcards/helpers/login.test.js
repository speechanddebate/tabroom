import { assert } from 'chai';
import pkg from 'uuid';
import config from '../../config/config';
import db from './db';
import login from './login';
import userData from '../../tests/testFixtures';

const { v4 } = pkg;

describe('Login Password Validation', () => {
	it('Authenticates the password correctly for a user', async () => {
		const req = {
			db,
			config,
			uuid     : v4(),
			params   : {
				email    : userData.testUser.email,
				password : userData.testPassword,
			},
		};

		const session = await login(req);
		assert.typeOf(session, 'object');
		assert.equal(session.person, '69');
		await session.destroy();
	});

	it('Rejects incorrect login for a user', async () => {
		const req = {
			db,
			config,
			uuid     : v4(),
			params   : {
				email    : userData.testUser.email,
				password : `${userData.testPassword}garbage`,
			},
		};

		const session = await login(req);
		assert.equal(session, 'Password was incorrect!');
	});
});
