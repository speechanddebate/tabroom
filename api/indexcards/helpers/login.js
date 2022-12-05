//
// Accept a email and password and verify that they're correct, and create &
// return a session object
//
import { b64_sha512crypt as crypt } from 'sha512crypt-node';

const login = async (req) => {

	const db = req.db;

	if (req.params.email && req.params.password) {

		const person = await db.person.findOne({
			where: { email: req.params.email },
			include : [
				{ model: db.personSetting, as: 'Settings' },
			],
		});

		if (typeof person === 'object') {

			const hash = crypt(req.params.password, person.password);

			if (hash !== person.password) {
				return 'Password was incorrect!';
			}

			const now = new Date();

			const userkey = crypt(req.uuid, person.password);

			const sessionTemplate = {
				person     : person.id,
				ip         : '127.0.0.1',
				created_at : now.toJSON(),
				userkey,
			};

			const session = await db.session.create(sessionTemplate);

			// Create the session in the database here after you test it.
			// oh. and figure out how to set a cookie.  sigh.

			// These extra hooks do not go into the database so set them
			// after.

			session.site_admin = person.site_admin;
			session.name = `${person.first} ${person.last}`;
			session.email = person.email;

			return session;

		}
		return `No user found for email ${req.params.email}`;
	}
};

export default login;
