import followers from '../../../../helpers/followers';
import { emailBlast, phoneBlast } from '../../../../helpers/mail.js';
import { sectionCheck, roundCheck } from '../../../../helpers/tourn-auth.js';

export const messageSection = {
	GET: async (req, res) => {
		if (!req.body) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		// Permissions.  I feel like there should be a better way to do this
		await sectionCheck(req, res, req.params.section_id);

		const messageData = await followers(
			{ sectionId : req.params.section_id },
			{ entries: true, no_followers: true }
		);

		messageData.text = req.body;

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

	GET: async (req, res) => {

		const permOK = await roundCheck(req, res, req.params.round_id);

		if (!permOK) {
			return;
		}

		if (!req.body.message) {
			res.status(200).json({ error: true, message: 'No message to blast sent' });
		}

		const messageData = await followers(
			{ roundId : req.params.round_id },
			{ entries: true, no_followers: true }
		);

		messageData.text = req.body;
		const emailResponse = await emailBlast(messageData);
		const phoneResponse = await phoneBlast(messageData);

		if (emailResponse.error && phoneResponse.error) {
			res.status(200).json({ error: true, message: emailResponse.message });
		} else {
			res.status(200).json({ error: false, message: `Message sent to ${emailResponse.count + phoneResponse.count} recipients` });
		}
	},
};
