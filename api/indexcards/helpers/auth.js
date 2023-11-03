// Parse the Tabroom cookies and determine whether there's an active session
// import { b64_sha512crypt as crypt } from 'sha512crypt-node';\

import { errorLogger } from './logger.js';
import getSettings from './settings.js';

export const auth = async (req) => {
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

			if (session.defaults) {
				try {
					session.defaults = JSON.parse(session.defaults);
				} catch (err) {
					errorLogger.info(`JSON parsing of defaults failed: ${err}`);
				}
			} else {
				session.defaults = {};
			}

			if (session.agent) {
				try {
					session.agent = JSON.parse(session.agent);
				} catch (err) {
					errorLogger.info(`JSON parsing of agent failed: ${err}`);
				}
			} else {
				session.agent = {};
			}

			if (session.Su)  {

				let realname = session.Person.first;
				if (session.Person.middle) {
					realname += session.Person.middle;
				}
				realname +=  session.Person.last;
				realname = `${session.Su.first} ${session.Su.last} as ${realname}`;

				session = {
					id          : session.id,
					person      : session.Person.id,
					site_admin  : session.Person.site_admin,
					email       : session.Person.email,
					name        : realname,
					su          : session.Su.id,
					defaults    : session.defaults,
					geoip       : session.geoip,
					agent       : session.agent_data,
					push_notify : session.push_notify,
				};

				session.settings = await getSettings(
					'person',
					session.person,
					{ skip: ['paradigm', 'paradigm_timestamp', 'nsda_membership'] },
				);

			} else if (session.Person) {

				let realname = session.Person.first;

				if (session.Person.middle) {
					realname += ` ${session.Person.middle}`;
				}
				realname += ` ${session.Person.last}`;
				session = {
					id          : session.id,
					person      : session.Person.id,
					site_admin  : session.Person.site_admin,
					email       : session.Person.email,
					name        : realname,
					defaults    : session.defaults,
					geoip       : session.geoip,
					agent       : session.agent_data,
					push_notify : session.push_notify,
				};

				session.settings = await getSettings(
					'person',
					session.person,
					{ skip: ['paradigm', 'paradigm_timestamp', 'nsda_membership'] },
				);
			}

			return session;
		}
	}
};

// Parse the Tabroom cookies and determine whether there's an active session

export const tournAuth = async function(req) {

	const tournId = parseInt(req.params.tourn_id);
	const session = req.session;

	if (session == null || Object.entries(session).length === 0) {
		return session;
	}

	if (typeof (tournId) === 'undefined') {
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
		session[tournId].level = 'owner';
		session[tournId].menu  = 'all';
		return session;
	}

	// Dost thou hath the keys to this gate?

	const result = await req.db.permission.findAll({
		where: { tourn: tournId, person: req.session.person },
	});

	if (result.count < 1) {

		delete session[tournId];

	} else {

		result.forEach(perm => {

			let current = '';
			if (session[tournId]) {
				current = session[tournId].level;
			}

			if (perm.tag === 'contact') {
				session[tournId].contact = true;
			} else if (

				perm.tag === 'owner'
				|| current === 'owner'
			) {

				// Nothing should override if I'm the owner already, so let's
				// just skip the rest and clear the flags
				session[tournId].level = 'owner';
				session[tournId].menu = 'all';
				delete session[tournId].events;

			} else if (
				perm.tag === 'tabber'
				|| current === 'tabber'
			) {
				session[tournId].level = 'tabber';
				session[tournId].menu  = 'all';
				delete session[tournId].events;

			} else if (
				perm.tag === 'by_event'
				|| current === 'by_event'
			) {
				session[tournId].level  = 'by_event';
				session[tournId].menu   = 'events';

			} else if (
				perm.tag === 'checker'
			) {
				session[tournId].level  = 'checker';
				session[tournId].menu   = 'none';
			}

			session[tournId].events = {};

			if (perm.details && (perm.tag === 'checker' || perm.tag === 'by_event')) {

				if (typeof perm.details === 'object') {
					session[tournId].events = perm.details;
				} else {
					try {
						session[tournId].events = JSON.parse(perm.details);
					} catch (err) {
						errorLogger.info(err);
					}
				}
			}
		});
	}

	return session;
};

export const checkJudgePerson = async (req, judgeId) => {

	if (req.session.site_admin) {
		return true;
	}

	const judge = await req.db.summon(req.db.judge, judgeId);

	if (judge.person === req.session.person) {
		return true;
	}

	return false;
};

export const checkPerms = async (req, res, query, replacements) => {

	if (req.session.site_admin) {
		return true;
	}

	const [permsData] = await req.db.sequelize.query(query, {
		replacements,
		type: req.db.sequelize.QueryTypes.SELECT,
	});

	if (!permsData) {
		return false;
	}

	if (permsData.site && permsData.timeslot) {

		const okEvents = await req.db.sequelize.query(`
			select
				distinct round(event) id
			from round
				where round.timeslot = :timeslotId
				and round.site = :siteId
		`, {
			replacements   : {
				timeslotId : permsData.timeslot,
				siteId     : permsData.site,
			},
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		// eslint-disable-next-line no-restricted-syntax
		for await (const event of okEvents) {
			if (!permsData.events) {
				permsData.events = [];
			}
			permsData.events.push(event.id);
		}
	}

	if (permsData.tourn !== parseInt(req.params.tourn_id)) {
		errorLogger.info({
			error     : true,
			message   : `You have a mismatch between the tournament element tourn ${permsData.tourn} and its parent tournament ${req.params.tourn_id}`,
		});
		return false;
	}

	if (req.session[permsData.tourn]) {
		if (req.session[permsData.tourn].level === 'owner') {
			return true;
		}

		if (
			req.session[permsData.tourn].level === 'tabber'
			&& req.threshold !== 'owner'
		) {
			return true;
		}

		if (
			req.session[permsData.tourn].level === 'checker'
			&& req.threshold !== 'tabber'
			&& req.threshold !== 'owner'
		) {
			return true;
		}

		if (req.session[permsData.tourn].level === 'by_event') {

			if (
				(req.threshold === 'tabber' || req.threshold === 'admin')
				&& req.session[permsData.tourn].events[permsData.event] === 'tabber'
			) {
				return true;
			}

			if (
				req.session[permsData.tourn].events
			) {

				if ( permsData.event
					&& req.session[permsData.tourn].events[permsData.event] === 'checker'
					&& req.threshold !== 'owner'
					&& req.threshold !== 'tabber'
				) {
					return true;
				}

				if ( permsData.event
					&& req.session[permsData.tourn].events[permsData.event] === 'tabber'
					&& req.threshold !== 'owner'
				) {
					return true;
				}

				if (permsData.events) {

					let OK = false;

					permsData.events.forEach( eventId => {

						if (req.session[permsData.tourn].events[eventId] === 'tabber'
							&& req.threshold !== 'owner'
						) {

							OK = true;
							return true;
						}

						if (req.session[permsData.tourn].events[eventId.toString()] === 'checker'
							&& req.threshold !== 'owner'
							&& req.threshold !== 'tabber'
						) {
							OK = true;
							return true;
						}
					});

					if (OK) {
						return true;
					}
				}
			}
		}
	}

	errorLogger.info({
		error     : true,
		message   : `You do not have permission to access that part of that tournament`,
	});

	return false;
};

export const sectionCheck = async (req, res, sectionId) => {

	const sectionQuery = `
		select event.tourn, event.id event
			from panel, round, event
		where panel.id = :sectionId
			and panel.round = round.id
			and round.event = event.id
	`;

	const replacements = { sectionId };
	return checkPerms(req, res, sectionQuery, replacements);
};

export const roundCheck = async (req, res, roundId) => {

	const roundQuery = `
		select event.tourn, event.id event
			from round, event
		where round.id = :roundId
			and round.event = event.id
	`;

	const replacements = { roundId };
	return checkPerms(req, res, roundQuery, replacements);
};

export const eventCheck = async (req, res, eventId) => {
	const eventQuery = `
		select event.tourn, event.id event
			from event
		where event.id = :eventId
	`;

	const replacements = { eventId };
	return checkPerms(req, res, eventQuery, replacements);
};

export const entryCheck = async (req, res, entryId) => {
	const entryQuery = `
		select event.tourn, entry.event
		from entry, event
		where entry.id = :entryId
			and entry.event = event.id
	`;

	const replacements = { entryId };
	return checkPerms(req, res, entryQuery, replacements);
};

export const schoolCheck = async (req, res, schoolId) => {
	const schoolQuery = `
		select school.tourn, school.id school
			from school
		where school.id = :schoolId
	`;

	const replacements = { schoolId };
	return checkPerms(req, res, schoolQuery, replacements);
};

export const timeslotCheck = async (req, res, timeslotId) => {
	const timeslotQuery = `
		select timeslot.tourn, timeslot.id timeslot
			from timeslot
		where timeslot.id = :timeslotId
	`;

	const replacements = { timeslotId };
	return checkPerms(req, res, timeslotQuery, replacements);
};

export const jpoolCheck = async (req, res, jpoolId) => {
	const jpoolQuery = `
		select category.tourn, jpool.id jpool, round.event event, st.value timeslot, jpool.site
			from (jpool, category)
				left join jpool_round jpr on jpr.jpool = jpool.id
				left join round on round.id = jpr.round
				left join jpool_setting st on st.tag = 'standby_timeslot' and st.jpool = jpool.id
		where jpool.id = :jpoolId
			and jpool.category = category.id
			group by jpool.id
	`;
	const replacements = { jpoolId };
	return checkPerms(req, res, jpoolQuery, replacements);
};

export const judgeCheck = async (req, res, judgeId) => {
	const judgeQuery = `
		select category.tourn, event.id event
			from category, event, judge
		where judge.id = :judgeId
			and judge.category = category.id
			and category.id = event.category
	`;

	const replacements = { judgeId };
	return checkPerms(req, res, judgeQuery, replacements);
};

export const categoryCheck = async (req, res, categoryId) => {
	const categoryQuery = `
		select category.tourn, event.id event
			from category, event
		where category.id = :categoryId
			and category.id = event.category
	`;

	const replacements = { categoryId };
	return checkPerms(req, res, categoryQuery, replacements);
};

export default auth;
