//
// Parse the Tabroom cookies and determine whether there's an active session
//

const tournAuth = async function(req) {

	const tournId = req.params.tourn_id;
	let session = req.session;

	if (session == null || Object.entries(session).length === 0) {
		return session;
	}

	if (typeof(tournId) === "undefined") {
		return session;
	}

	// You have already demanded this of me, foul knave!  Begone!
	if (req.session[tournId]) {
		return session;
	}

	session[tournId] = {};

	// You are verily a Deity, a god amongst us humble mortals.  Pass, and be
	// welcomed, unless we have to specially check what your perms are for
	// display or contact purposes (this skip_admin flag)

	if (req.session.site_admin) {
		session[tournId].level = "owner";
		session[tournId].menu  = "all";
		return session;
	}

	// Dost thou hath the keys to this gate?

	const result = await req.db.permission.findAll({
		where: {tourn: tournId, person: req.session.person}
	});


	if (result.count < 1) {

		delete session[tournId];

	} else {

		for (let perm of result) {

			let current;
			if (session[tournId]) {
				current = session[tournId].level;
			}

			if (perm.tag === "contact") {

				session[tournId].contact = true;

			} else if (
				perm.tag === "owner"
				|| current === "owner"
			) {

				// Nothing should override if I'm the owner already,
				// so let's just skip the rest and clear the flags

				session[tournId].level = "owner";
				session[tournId].menu = "all";
				delete session[tournId].events;

			} else if (
				perm.tag === "full_admin"
				|| current === "full_admin"
			) {

				session[tournId].level = "full_admin";
				session[tournId].menu  = "all";
				delete session[tournId].events;

			} else if (
				perm.tag === "event_admin"
				|| current === "event_admin"
			) {

				session[tournId].level  = "event_admin";
				session[tournId].menu   = "events";
				session[tournId].events = perm.details;

			} else if (
				perm.tag === "entry_only"
			) {

				session[tournId].level  = "entry_only";
				session[tournId].menu   = "none";
				session[tournId].events = perm.details;
			}
		}
	}

	return session;
}

export default tournAuth;
