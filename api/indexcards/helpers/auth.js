//
// Parse the Tabroom cookies and determine whether there's an active session
//

import config from '../../config/config';
import db from '../models';

const env = process.env.NODE_ENV || 'development';

const auth = async function(req, res, next) {

	if (req.cookies[config.COOKIE_NAME]) {

		const session = await db.session.findOne({
			where: { userkey: req.cookies[config.cookieName] },
			include: Person, Su
		});

		if (session) {

			if (session.su)  {

				let realname = session.su.first;
				if (session.su.middle) {
					realname += " "+session.su.middle;
				}
				realname += " "+session.su.last;

				req.session = {
					id         : session.id,
					person     : session.su.id,
					site_admin : session.su.site_admin,
					defaults   : session.defaults,
					email      : session.su.email,
					name       : realname,
					su         : session.person.id
				};

			} else {

				let realname = session.person.first;
				if (session.person.middle) {
					realname += " "+session.person.middle;
				}
				realname += " "+session.person.last;

				req.session = {
					id         : session.id,
					person     : session.person.id,
					site_admin : session.person.site_admin,
					defaults   : session.defaults,
					email      : session.person.email,
					name       : realname
				};
			}

			next();

		} else {

			req.session = {};
			console.log("No valid session found for key");
			console.log(req.cookies[config.COOKIE_NAME]);
			next();
		}

	} else {

		req.session = {};
		console.log("No valid session cookie found");
		next();
	}
}

export default auth;
