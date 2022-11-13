import { randomPhrase, roundName } from '@speechanddebate/nsda-js-utils';
import selectPanelEmail from './selectPanelEmail';
import sendMail from './mail';
import config from '../../../../config/config';
import { debugLogger } from '../../../helpers/logger';

const postShare = {
	POST: async (req, res) => {
		const db = req.db;
		if (req.body.share_key !== config.SHARE_KEY) {
			return res.status(401).json({ message: 'Invalid share key' });
		}
		if (!req.body.panels || req.body.panels.length < 1) {
			return res.status(400).json({ message: 'Must provide an array of panels' });
		}

		const enabled = await db.sequelize.query(`
			select value from tabroom_setting where tag = 'share_email_enabled'
		`);

		if (!enabled || !enabled[0] || !enabled[0]?.[0]?.value) {
			return res.status(200).json({ message: 'Tabroom share emails disabled' });
		}

		// If passphrase panel ID and a file are provided, just forward the file to the panel
		if (req.body.panels.length === 1 && typeof req.body.panels[0] === 'string' && req.body.files && req.body.files.length > 0) {
			const result = await selectPanelEmail(req.body.panels[0]);

			if (!result || result.length < 1) {
				return res.status(400).json({
					message: 'No current round with that name found',
				});
			}

			const emails = result.map(r => r.email).filter(email => email !== req.body.from);
			const tournament = result[0].tournament;
			const round = result[0].round;
			const room = result[0].room;

			if (!emails || emails.length < 1) {
				return res.status(400).json({
					message: 'No emails found for the round, nothing to send',
				});
			}

			try {
				debugLogger.info(`Sending single panel email for ${room} to ${emails} with ${req.body.files.length} attachments`);
				await sendMail(
					`${room}@share.tabroom.com`,
					`${emails}`,
					`${tournament} ${roundName(round)} (${room}) - Speech Documents`,
					`Share speech documents for this round (10mb limit, docs only) by replying to this email (must include an attachment), or going to https://share.tabroom.com/${room}`,
					null,
					req.body.files || [],
				);
				return res.status(201).json({ message: 'Successfully sent speech doc email' });
			} catch (err) {
				return res.status(500).json({ message: 'Failed to send speech doc email' });
			}
		}

		// Otherwise generate a passphrase and initiate a chain to every panel
		const emailPromises = [];
		const panels = req.body.panels.filter(p => typeof p === 'number');

		panels.forEach(async (p) => {
			const phrase = randomPhrase();
			await db.sequelize.query(`
				insert into panel_setting (panel, tag, value)
				values (?, 'share', ?)
				on duplicate key update
				value = ?
			`, { replacements: [p, phrase, phrase] });

			const result = await selectPanelEmail(phrase);

			if (result.length > 0) {
				const emails = result.map(r => r.email);
				const tournament = result[0].tournament;
				const round = result[0].round;

				debugLogger.info(`Sending bulk panel email for panel ${p} (${phrase}) to ${emails}`);
				emailPromises.push(
					sendMail(
						`${phrase}@share.tabroom.com`,
						`${emails}`,
						`${tournament} ${roundName(round)} (${phrase}) - Speech Documents`,
						`Share speech documents for this round (10mb limit, docs only) by replying to this email (must include an attachment), or going to https://share.tabroom.com/${phrase}`,
					)
				);
			}
		});

		await Promise.all(emailPromises);

		return res.status(201).json({ message: 'Successfully sent speech doc emails' });
	},
};

postShare.POST.apiDoc = {
	summary: 'Creates a speech doc chain for an array of panels, including sending a file to an individual panel',
	operationId: 'postShare',
	requestBody: {
		description: 'Share request',
		required: true,
		content: { '*/*': { schema: { $ref: '#/components/schemas/Share' } } },
	},
	responses: {
		201: {
			description: 'Success',
			content: {
				'*/*': {
					schema: {
						type: 'string',
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['share'],
};

export default postShare;
