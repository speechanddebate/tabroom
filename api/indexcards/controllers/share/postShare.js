import crypto from 'crypto';
import { randomPhrase } from '@speechanddebate/nsda-js-utils';
import selectPanelEmail from './selectPanelEmail.js';
import sendMail from './mail.js';
import config from '../../../config/config.js';
import db from '../../models/index.cjs';

const postShare = {
	POST: async (req, res) => {
		const hash = crypto.createHash('sha256').update(config.SHARE_KEY).digest('hex');
		if (req.body.share_key !== hash) {
			return res.status(401).json({ message: 'Invalid share key' });
		}
		if (!req.body.panel && !req.body.round_id) {
			return res.status(400).json({ message: 'Must provide either a panel or round_id' });
		}

		// If panel is provided, just forward the file to the panel
		if (req.body.panel) {
			const result = await selectPanelEmail(req.body.panel);

			if (!result || result.length < 1) {
				return res.status(400).json({
					message: 'No round with that name found, no emails in round, or round is more than a day ago',
				});
			}

			const emails = result.map(r => r.email);
			const tournament = result[0].tournament;
			const round = result[0].round;

			try {
				await sendMail(
					`${req.body.panel}@share.tabroom.com`,
					`${emails}`,
					`Speech Documents - ${tournament} Round ${round} (${req.body.panel})`,
					'Speech Documents',
					null,
					{ filename: req.body.filename, file: req.body.file },
				);
				return res.status(201).json({ message: 'Successfully sent speech doc email' });
			} catch (err) {
				return res.status(500).json({ message: 'Failed to send speech doc email' });
			}
		}

		// Otherwise initiate a chain to every panel in the round
		const panels = await db.sequelize.query(`
			SELECT DISTINCT panel.id FROM panel WHERE round = ?
		`, { replacements: [req.body.round_id] });

		const emailPromises = [];
		panels.forEach(async (p) => {
			const phrase = randomPhrase();
			await db.sequelize.query(`
				insert into panel_setting (panel, tag, value)
				values (?, 'share', ?)
				on duplicate key update
				value = ?
			`, { replacements: [p.id, phrase] });

			const result = await selectPanelEmail(phrase);

			if (result.length > 0) {
				const emails = result.map(r => r.email);
				const tournament = result[0].tournament;
				const round = result[0].round;
				emailPromises.push(
					sendMail(`${phrase}@tabroom.com`, `${emails}`, `Speech Documents - ${tournament} Round ${round} (${phrase})`, 'Speech Documents')
				);
			}
		});

		await Promise.All(emailPromises);

		return res.status(201).json({ message: 'Successfully sent speech doc emails' });
	},
};

postShare.POST.apiDoc = {
	summary: 'Creates a speech doc chain for each panel in a round or sends a file to an individual panel',
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
