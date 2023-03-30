// Parse the Tabroom cookies and determine whether there's an active session
// import { b64_sha512crypt as crypt } from 'sha512crypt-node';

const auth = async (req) => {
	const db = req.db;

	if (req.session && req.session.id) {
		return req.session;
	}

	const cookie = req.cookies[req.config.COOKIE_NAME] || req.headers['x-tabroom-cookie'];

	if (cookie) {

		let session = await db.session.findOne({
			where: {
				userkey: cookie,
			},
			include : [
				{ model: db.person, as: 'Person' },
				{ model: db.person, as: 'Su' },
			],
		});

		if (session) {

			session.Su = await session.getSu();

			if (session.Su)  {

				let realname = session.Person.first;

				if (session.Person.middle) {
					realname += ` ${session.Person.middle}`;
				}
				realname += ` ${session.Person.last}`;

				session = {
					id         : session.id,
					person     : session.Person.id,
					site_admin : session.Person.site_admin,
					defaults   : session.defaults,
					email      : session.Person.email,
					name       : realname,
					su         : session.Su.id,
				};

			} else if (session.Person) {

				let realname = session.Person.first;

				if (session.Person.middle) {
					realname += ` ${session.Person.middle}`;
				}

				realname += ` ${session.Person.last}`;

				session = {
					id         : session.id,
					person     : session.Person.id,
					site_admin : session.Person.site_admin,
					defaults   : session.defaults,
					email      : session.Person.email,
					name       : realname,
				};
			}

			return session;
		}
	}
};

export default auth;
