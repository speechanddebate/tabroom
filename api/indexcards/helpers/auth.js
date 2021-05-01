//
// Parse the Tabroom cookies and determine whether there's an active session
//

const auth = async function(req, res) {

	const db = req.db;

	if (req.session && req.session.id) {
		return req.session;
	}

	if (req.cookies[req.config.COOKIE_NAME]) {

		let session = await db.session.findOne({
			where: { userkey: req.cookies[req.config.COOKIE_NAME] },
			include : [
				{ model: db.person, as: 'Person'},
				{ model: db.person, as: 'Su'}
			]
		});

		if (session) {

			session.Su = await session.getSu();

			if (session.Su)  {

				let realname = session.Su.first;

				if (session.Su.middle) {
					realname += " "+session.Su.middle;
				}
				realname += " "+session.Su.last;

				session = {
					id         : session.id,
					person     : session.Su.id,
					site_admin : session.Su.site_admin,
					defaults   : session.defaults,
					email      : session.Su.email,
					name       : realname,
					su         : session.Person.id
				};

			} else if (session.Person) {

				let realname = session.Person.first;

				if (session.Person.middle) {
					realname += " "+session.Person.middle;
				}

				realname += " "+session.Person.last;

				session = {
					id         : session.id,
					person     : session.Person.id,
					site_admin : session.Person.site_admin,
					defaults   : session.defaults,
					email      : session.Person.email,
					name       : realname
				};
			}

			return session;

		} else {

			console.log("No valid Tabroom session found for key");
			console.log(req.cookies[req.config.COOKIE_NAME]);
			return;
		}

	} else {
		console.log("No valid Tabroom session cookie found");
		return;
	}
}

export default auth;
