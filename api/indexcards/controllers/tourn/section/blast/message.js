import { getFollowers, getJPoolJudges, getTimeslotJudges } from '../../../../helpers/followers';
import { emailBlast, phoneBlast } from '../../../../helpers/mail.js';
import { sectionCheck, jpoolCheck, timeslotCheck, roundCheck } from '../../../../helpers/auth.js';
import { errorLogger } from '../../../../helpers/logger';
import notify from '../../../../helpers/pushNotify';
import objectify from '../../../../helpers/objectify';

export const messageSection = {
	POST: async (req, res) => {
		if (!req.body.message) {
			res.status(200).json({
				error   : true,
				message : 'No message to blast sent',
			});
		}

		// Permissions.  I feel like there should be a better way to do this
		const permOK = await sectionCheck(req, res, req.params.sectionId);
		if (!permOK) {
			return;
		}

		const personIds = await getFollowers(
			{ sectionId : req.params.sectionId },
			req.body
		);

		const notifyResponse = await notify({
			ids  : personIds,
			text : req.body.message,
		});

		if (notifyResponse.error) {
			errorLogger.error(notifyResponse.message);
			res.status(200).json(notifyResponse);
		} else {

			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${notifyResponse.push?.count || 0} recipients`,
				person      : req.session.person,
				count       : notifyResponse.push?.count || 0,
				panel       : req.params.sectionId,
			});

			await req.db.changeLog.create({
				tag         : 'emails',
				description : `${req.body.message} sent to ${notifyResponse.email?.count || 0}`,
				person      : req.session.person,
				count       : notifyResponse.email?.count || 0,
				panel       : req.params.sectionId,
			});

			res.status(200).json({
				error   : false,
				message : notifyResponse.message,
			});
		}
	},
};

export const messageRound = {

	POST: async (req, res) => {

		const permOK = await roundCheck(req, res, req.params.roundId);

		if (!permOK) { return; }

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const personIds = await getFollowers(req.body);

		const notifyResponse = await notify({
			ids  : personIds,
			text : req.body.message,
		});

		if (notifyResponse.error) {
			errorLogger.error(notifyResponse.message);
			res.status(200).json(notifyResponse);
		} else {

			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${notifyResponse.push?.count || 0} recipients`,
				person      : req.session.person,
				count       : notifyResponse.push?.count || 0,
				panel       : req.params.roundId,
			});

			await req.db.changeLog.create({
				tag         : 'emails',
				description : `${req.body.message} sent to ${notifyResponse.email?.count || 0}`,
				person      : req.session.person,
				count       : notifyResponse.email?.count || 0,
				panel       : req.params.roundId,
			});

			res.status(200).json({
				error   : false,
				message : notifyResponse.message,
			});
		}
	},
};

export const messageTimeslot = {

	POST: async (req, res) => {

		const permOK = await timeslotCheck(req, res, req.params.timeslotId);
		if (!permOK) { return; }

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		req.body.timeslotId = req.params.timeslotId;
		delete req.body.roundId;

		const personIds = await getFollowers(req.body);
		const notifyResponse = await notify({
			ids  : personIds,
			text : req.body.message,
		});

		if (notifyResponse.error) {
			errorLogger.error(notifyResponse.message);
			res.status(200).json(notifyResponse);
		} else {

			const rounds = await req.db.round.findAll({
				where: { timeslot: req.params.timeslotId },
			});

			rounds.map( async (round) => {

				const blastLog = await req.db.changeLog.create({
					tag         : 'blast',
					description : `${req.body.message} sent to whole timeslot. ${notifyResponse.push?.count || 0} recipients`,
					person      : req.session.person,
					count       : notifyResponse.push?.count || 0,
					round       : round.id,
				});

				await req.db.changeLog.create({
					tag         : 'emails',
					description : `${req.body.message} sent to whole timeslot. ${notifyResponse.email?.count || 0}`,
					person      : req.session.person,
					count       : notifyResponse.email?.count || 0,
					round       : round.id,
				});

				return blastLog;
			});

			res.status(200).json({
				...notifyResponse,
			});
		}
	},
};

// These two still require conversion to the new interface with push notifications.

export const messageJPool = {

	POST: async (req, res) => {

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const permOK = await jpoolCheck(req, res, req.params.jpoolId);

		if (!permOK) {
			return;
		}

		const poolJudges = await getJPoolJudges(
			{ jpoolId : req.params.jpoolId }
		);

		const jpool = await req.db.summon(req.db.jpool, req.params.jpoolId);
		const tourn = await req.db.summon(req.db.tourn, req.params.tournId);
		let recipients = 0;
		let errors = '';

		Object.keys(poolJudges?.only?.judge).forEach( async (judgeId) => {

			const judge = poolJudges.only.judge[judgeId];
			const messageData = {
				text    : '',
				html    : '',
				subject : `${judge.name} judge pool ${jpool.name}`,
				...judge,
			};

			if (tourn.webname) {
				messageData.from = `${tourn.name} <${tourn.webname}@www.tabroom.com>`;
			}

			messageData.text = `\nJudge ${judge.name}\nPool ${jpool.name}\n${req.body.message}`;
			messageData.html = `<p style='padding-top: 4px;'>Pool: ${jpool.name}</p>`;
			messageData.html += `<p style='padding-top: 4px;'>Judge: ${judge.name}</p>`;
			messageData.html += `<p style='padding-top: 4px;'>${req.body.message}</p>`;

			try {
				const emailResponse = await emailBlast(messageData);
				const phoneResponse = await phoneBlast(messageData);
				recipients += emailResponse ? emailResponse.count : 0;
				recipients += phoneResponse ? phoneResponse.count : 0;
			} catch (err) {
				errors += ` ${err}`;
			}
		});

		const jpoolRoundQuery = `
			select distinct round.id
				from round, jpool_round jpr
			where jpr.jpool = :jpoolId
				and jpr.round = round.id
		`;

		const rawRounds = await req.db.sequelize.query(jpoolRoundQuery, {
			replacements: { jpoolId: req.params.jpoolId },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		rawRounds.forEach( async (round) => {
			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${jpool.name} judges`,
				person      : req.session.person,
				count       : recipients,
				round       : round.id,
			});
		});

		if (errors) {
			res.status(200).json({
				error   : true,
				message : `Message sent to ${recipients} recipients but with errors ${errors}`,
			});

		} else {
			res.status(200).json({
				error   : false,
				message : `Message sent to ${recipients} recipients`,
			});
		}
	},
};

export const messageFree = {

	POST: async (req, res) => {

		// This is a complicated one.  Given a timeslot ID and site ID, the aim
		// here is to individually message every judge who IS in the pools that
		// the rounds tied to that timeslot pull from, but who is NOT either
		// judging, or in a standby timeslot attached to this timeslot.  In
		// other words, they are completely free to go and frolic or whatevs.

		// Yes I just used the world frolic.  Fight me.

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		let permOK = false;

		if (req.body.jpoolId) {
			permOK = await jpoolCheck(req, res, req.body.jpoolId);
		} else if (req.params.timeslotId) {
			permOK = await timeslotCheck(req, res, req.body.timeslotId);
		}

		if (!permOK) {
			return false;
		}

		// I'll take this in parts.  First pull all the judges in the relevant
		// pools into allJudges

		const allJudgesQuery = `
			select
				judge.id, judge.first, judge.last
			from judge, jpool_judge jpj, jpool_round jpr, round

			where round.timeslot = :timeslotId
				and round.site   = :siteId
				and round.id     = jpr.round
				and jpr.jpool    = jpj.jpool
				and jpj.judge    = judge.id
				and judge.active = 1
		`;

		const allRawJudges = await req.db.sequelize.query(allJudgesQuery, {
			replacements: { ...req.body },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		// Next pull the judges who are actively judging in this timeslot into
		// isJudging

		const isJudgingQuery = `
			select
				judge.id
			from round, panel, ballot, judge

			where round.timeslot = :timeslotId
				and round.site = :siteId
				and round.id = panel.round
				and panel.id = ballot.panel
				and ballot.judge = judge.id
		`;

		const isJudging = await req.db.sequelize.query(isJudgingQuery, {
			replacements: { ...req.body },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		// Next pull any judges in a standby pool for this timeslot into
		// isStandby

		const isStandbyQuery = `
			select
				judge.id
			from judge, jpool_judge jpj, jpool_setting jps
				where jps.value = :timeslotId
				and jps.tag = 'standby_timeslot'
				and jps.jpool = jpj.jpool
				and jpj.judge = judge.id
		`;

		const isStandby = await req.db.sequelize.query(isStandbyQuery, {
			replacements: { ...req.body },
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		// Remove the isJudging and isStandby members from the allJudges
		const allJudges = objectify(allRawJudges);

		for await (const standby of isStandby) {
			delete allJudges[standby.id];
		}

		for await (const judging of isJudging) {
			delete allJudges[judging.id];
		}

		// Send Individualized (NAME!) blasts to the judges who are neither
		// active nor on standby. The name is included because otherwise judges
		// following other judges from their school will claim they were told
		// they were free when nope, NOPE, NOOOOOOPE.

		const judgeBlasts = await getTimeslotJudges({
			timeslotId : req.body.timeslotId,
			siteId     : req.body.siteId,
		});

		let recipients = 0;
		let errors = '';

		for await (const judgeId of Object.keys(allJudges)) {

			const judge = allJudges[judgeId];
			const messageData = { ...judgeBlasts.only.judge[judgeId] };

			messageData.subject = `Judge ${judge.first} ${judge.last} Released`;
			messageData.text = `\n\nJudge: ${judge.first} ${judge.last}\n`;
			messageData.text += `${req.body.message}`;
			messageData.html = `<p style='padding-top: 8px;'>JUDGE: ${judge.first} ${judge.last}</p>`;
			messageData.html += `<p style='padding-top: 4px;'>${req.body.message}</p>`;

			try {
				const emailResponse = await emailBlast(messageData);
				const phoneResponse = await phoneBlast(messageData);
				recipients += emailResponse ? emailResponse.count : 0;
				recipients += phoneResponse ? phoneResponse.count : 0;
			} catch (err) {
				errors += ` ${err}`;
			}
		}

		const rounds = await req.db.round.findAll({
			where : {
				timeslot : req.body.timeslotId,
				site     : req.body.siteId,
			},
		});

		rounds.forEach( async (round) => {
			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${recipients} judges`,
				person      : req.session.person,
				count       : recipients,
				round       : round.id,
			});
		});

		if (errors) {
			res.status(200).json({
				error   : true,
				message : `Message sent to ${recipients} recipients with errors ${errors}`,
			});
		} else {
			res.status(200).json({
				error   : false,
				message : `Message sent to ${recipients} free recipients`,
			});
		}
	},
};
