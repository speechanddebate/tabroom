import getFollowers from '../../../../helpers/followers';
import { emailBlast, phoneBlast } from '../../../../helpers/mail.js';
import { sectionCheck, timeslotCheck, roundCheck } from '../../../../helpers/tourn-auth.js';

export const messageSection = {
	POST: async (req, res) => {
		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		// Permissions.  I feel like there should be a better way to do this
		const permOK = await sectionCheck(req, res, req.params.section_id);
		if (!permOK) {
			return;
		}

		const messageData = await getFollowers(
			{ sectionId : req.params.section_id },
			{ entries: true, no_followers: req.body.no_followers }
		);

		messageData.text = `\n ${req.body.message}`;
		messageData.html = `<p>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';
		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {
			res.status(200).json({ error: false, message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients` });
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

		messageData.text = `\n ${req.body.message}`;
		messageData.html = `<p>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';
		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {
			res.status(200).json({ error: false, message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients` });
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
			{ roundId : req.params.round_id },
			{
				recipients   : req.body.recipients,
				status       : req.body.status,
				flight       : req.body.flight,
				no_followers : req.body.no_followers,
			}
		);

		messageData.text = `\n ${req.body.message}`;
		messageData.html = `<p>${req.body.message}</p>`;
		messageData.subject = 'Message from Tab';
		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {
			res.status(200).json({ error: false, message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients` });
		}
	},
};
