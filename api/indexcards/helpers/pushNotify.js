import axios from 'axios';
import db from './db.js';
import emailBlast from './mail.js';
import config from '../../config/config';

export const notify = async (inputData) => {

	const pushReply = {
		web   : {},
		email : {},
	};

	pushReply.web = await pushNotify(inputData);
	pushReply.email = await emailNotify(inputData);

	const reply = {
		error   : pushReply.web?.error || pushReply.email?.error,
		message : `${pushReply.web.message} and ${pushReply.email.message}`,
		email   : pushReply.email,
		web     : pushReply.web,
	};

	return reply;
};

export const pushNotify = async (inputData) => {

	if (!inputData.ids || !inputData.text) {
		return {
			error   : false,
			message : `No receipients or message sent for push noitications`,
			count   : inputData.ids.length,
		};
	}

	const recipients = await db.sequelize.query(`
		select
			person.id, person.first, person.last, person.no_email,
			push_notify.value
		from person, person_setting push_notify
		where person.id IN (:personIds)
			and person.id = push_notify.person
			and push_notify.tag = 'push_notify'
	`, {
		replacements: { personIds: inputData.ids },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const targetPromise = recipients.map( async (person) => {
		if (person.id === 1) {
			// it won't let you use 1 or 0 as user IDs which REALLY PISSED PALMER OFF.
			return '100';
		}
		return `${person.id}`;
	});

	let targetIds = await Promise.all(targetPromise);

	if (process.env.NODE_ENV !== 'production') {
		targetIds = ['100'];
	}

	if (inputData.append) {
		inputData.text += `\n${inputData.append}`;
	}

	if (targetIds && targetIds.length > 0) {

		const notification = {
			app_id          : config.ONE_SIGNAL.app_id,
			name            : 'Test Blast for New Tabroom',
			url 		    : inputData.url ? inputData.url : 'https://www.tabroom.com/user/home.mhtml',
			contents        : { en: inputData.text },
			headings        : { en: inputData.subject ? inputData.subject : 'Message from Tab' },
			include_aliases: {
				external_id: targetIds,
			},
			target_channel  : 'push',
		};

		const reply = await axios.post(
			'https://onesignal.com/api/v1/notifications',
			notification,
			{
				headers : {
					Authorization  : `Basic ${config.ONE_SIGNAL.appKey}`,
					'Content-Type' : 'application/json',
					Accept         : 'application/json',
				},
			},
		);

		if (reply.status === 200) {
			return {
				error   : false,
				message : `Push notification sent to ${targetIds ? targetIds.length : 0} recipients`,
				count   : targetIds.length,
			};
		}

		return {
			error   : true,
			message : `Push notification failed with status ${reply.status} and text ${reply.statusText}`,
		};
	}

	return {
		error   : false,
		message : `No recipients found for the push notification`,
		count   : 0,
	};
};

export const emailNotify = async (inputData) => {

	if (!inputData.ids || !inputData.text) {
		return {
			error   : false,
			message : `No receipients or message sent for push noitications`,
			count   : inputData.ids.length,
		};
	}

	const recipients = await db.sequelize.query(`
		select
			person.id, person.first, person.last, person.email
		from person
		where person.id IN (:personIds)
			and person.no_email != 1
	`, {
		replacements: { personIds: inputData.ids },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const emailPromise = recipients.map( async (person) => {
		return person.email;
	});

	inputData.email = await Promise.all(emailPromise);

	if (inputData.email && inputData.email.length > 0) {
		const emailResponse = await emailBlast(inputData);

		if (emailResponse.error) {
			return {
				error   : true,
				message : emailResponse.message,
			};
		}

		return {
			error   : false,
			message : `Message emailed to ${inputData.email.length} recipients `,
			count   : inputData.email.length,
		};
	}

	return {
		error   : true,
		message : `No recipients found for the email notification`,
	};
};

export default notify;
