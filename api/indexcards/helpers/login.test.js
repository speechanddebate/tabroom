import config from '../../config/config'
import db from '../models';
import login from './login';

import { assert } from 'chai';
import httpMocks from 'node-mocks-http';
import userData from '../tests/users';
import uuid from 'uuid/v4';

describe ("Login Password Validation", () => {

	let testUser = {};
	let testUserSession = {};

	before("Set Dummy Data", async () => {
		testUser = await db.person.create(userData.testUser);
		testUserSession = await db.session.create(userData.testUserSession);
	});

	it("Authenticates the password correctly for a user", async () => {
		let req = {
			db       : db,
			config   : config,
			uuid     : uuid(),
			params   : {
				email : userData.testUser.email,
				password : userData.testPassword,
			},
		};

		const session = await(login(req));

		assert.typeOf(session, 'object');
		assert.equal(session.person, '69');
		testUser.session = session;

	});

	it("Rejects incorrect login for a user", async () => {
		let req = {
			db       : db,
			config   : config,
			uuid     : uuid(),
			params   : {
				email : userData.testUser.email,
				password : userData.testPassword+"garbage",
			},
		};

		const session = await(login(req));
		assert.equal(session, 'Password was incorrect!');
	});
	after("Remove Dummy Data", async () => {
		await testUser.destroy();
		await testUserSession.destroy();
	});
});

