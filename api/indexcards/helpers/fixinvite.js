/* eslint-disable camelcase */
import { debugLogger } from './logger.js';
import db from './db.js';

export const fixInvite = {
	GET: async (req, res) => {

		const existingFiles = await db.sequelize.query(`
			select ts.tourn, ts.tag, ts.value, ts.timestamp
				from tourn_setting ts
			where ts.tag IN ('bills', 'invite')
			order by ts.timestamp DESC
		`, {
			type : db.sequelize.QueryTypes.SELECT,
		});

		const results = [];

		existingFiles.forEach( async invite => {

			invite.page_order = 1;

			if (invite.tag === 'bills') {
				invite.page_order = 2;
			}

			invite.filename = invite.value;
			delete invite.value;

			debugLogger.info(invite);

			results.push(invite);

			//	const inviteFile = await db.File.create(invite);
			//	debugLogger.info(`I have created an invite file ${inviteFile.id} for tourn ${invite.tourn}`);

		});

		res.json(results);
	},
};

export default fixInvite;
