import { getFollowers, getJPoolJudges, getTimeslotJudges } from '../../../../helpers/followers';
import { emailBlast, phoneBlast } from '../../../../helpers/mail.js';
import { sectionCheck, jpoolCheck, timeslotCheck, roundCheck } from '../../../../helpers/auth.js';
import objectify from '../../../../helpers/objectify';

export const messageSection = {
	POST: async (req, res) => {
		if (!req.body.message) {
			res.status(200).json({
				error: true,
				message: 'No message to blast sent',
			});
		}

		// Permissions.  I feel like there should be a better way to do this
		const permOK = await sectionCheck(req, res, req.params.section_id);
		if (!permOK) {
			return;
		}

		const messageData = await getFollowers(
			{ sectionId : req.params.section_id },
			{
				recipients   : req.body.recipients,
				status       : req.body.status,
				flight       : req.body.flight,
				no_followers : req.body.no_followers,
			}
		);

		messageData.text = `\n\n${req.body.message}`;
		messageData.html = `<p style='padding-top: 8px;'>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';

		const tourn = await req.db.summon(req.db.tourn, req.params.tourn_id);
		if (tourn.webname) {
			messageData.from = `${tourn.name} <${tourn.webname}@www.tabroom.com>`;
		}

		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {

			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${req.body.recipients}`,
				person      : req.session.person,
				count       : phoneResponse.count,
				panel       : req.params.section_id,
			});

			await req.db.changeLog.create({
				tag         : 'emails',
				description : `${req.body.message} sent to ${req.body.recipients}`,
				person      : req.session.person,
				count       : emailResponse.count,
				panel       : req.params.section_id,
			});

			res.status(200).json({
				error   : false,
				message : `Message sent to ${emailResponse.count + phoneResponse.count} recipients`,
			});
		}
	},
};

export const messageRound = {

	POST: async (req, res) => {

		const permOK = await roundCheck(req, res, req.params.round_id);

		if (!permOK) {
			return;
		}

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const messageData = await getFollowers(
			{ roundId : req.params.round_id },
			{
				recipients   : req.body.recipients,
				status       : req.body.status,
				flight       : req.body.flight,
				no_followers : req.body.no_followers,
			}
		);

		messageData.text = `\n\n${req.body.message}`;
		messageData.html = `<p style='padding-top: 8px;'>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';

		const tourn = await req.db.summon(req.db.tourn, req.params.tourn_id);
		if (tourn.webname) {
			messageData.from = `${tourn.name} <${tourn.webname}@www.tabroom.com>`;
		}

		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {

			await req.db.changeLog.create({
				tag         : 'blast',
				description : `${req.body.message} sent to ${req.body.recipients} in ${req.body.status} sections`,
				person      : req.session.person,
				count       : phoneResponse.count,
				round       : req.params.round_id,
			});

			await req.db.changeLog.create({
				tag         : 'emails',
				description : `${req.body.message} sent to ${req.body.recipients} in ${req.body.status} sections`,
				person      : req.session.person,
				count       : emailResponse.count,
				round       : req.params.round_id,
			});

			res.status(200).json({
				error: false,
				message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients`,
			});
		}
	},
};

export const messageTimeslot = {

	POST: async (req, res) => {

		const permOK = await timeslotCheck(req, res, req.params.timeslot_id);

		if (!permOK) {
			return;
		}

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const messageData = await getFollowers(
			{ timeslotId : req.params.timeslot_id },
			{
				recipients   : req.body.recipients,
				status       : req.body.status,
				flight       : req.body.flight,
				no_followers : req.body.no_followers,
			}
		);

		messageData.text = `\n\n${req.body.message}`;
		messageData.html = `<p style='padding-top: 8px;'>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';

		const tourn = await req.db.summon(req.db.tourn, req.params.tourn_id);
		if (tourn.webname) {
			messageData.from = `${tourn.name} <${tourn.webname}@www.tabroom.com>`;
		}

		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {

			const rounds = await req.db.round.findAll({
				where: { timeslot: req.params.timeslot_id },
			});

			rounds.forEach( async (round) => {
				await req.db.changeLog.create({
					tag         : 'blast',
					description : `${req.body.message} sent to ${req.body.recipients} in ${req.body.status} sections`,
					person      : req.session.person,
					count       : phoneResponse.count,
					round       : round.id,
				});

				await req.db.changeLog.create({
					tag         : 'emails',
					description : `${req.body.message} sent to ${req.body.recipients} in ${req.body.status} sections`,
					person      : req.session.person,
					count       : emailResponse.count,
					round       : round.id,
				});
			});
			res.status(200).json({ error: false, message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients` });
		}
	},
};

export const messageJPool = {

	POST: async (req, res) => {

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const permOK = await jpoolCheck(req, res, req.params.jpool_id);

		if (!permOK) {
			return;
		}

		const poolJudges = await getJPoolJudges(
			{ jpoolId : req.params.jpool_id }
		);

		const jpool = await req.db.summon(req.db.jpool, req.params.jpool_id);
		const tourn = await req.db.summon(req.db.tourn, req.params.tourn_id);
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
			replacements: { jpoolId: req.params.jpool_id },
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
