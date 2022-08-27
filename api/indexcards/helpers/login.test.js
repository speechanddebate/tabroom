import { assert } from 'chai';
import pkg from 'uuid';
import config from '../../config/config.cjs';
import db from '../models/index.cjs';
import login from './login.js';
import userData from '../../tests/testFixtures.js';

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
