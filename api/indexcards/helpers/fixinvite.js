/* eslint-disable camelcase */
import db from './db.js';
import s3Client from './aws.js';

export const fixInvite = {
	GET: async (req, res) => {

		const existingFiles = await db.sequelize.query(`
			select ts.tourn, ts.tag, ts.value, ts.timestamp
				from tourn_setting ts
			where ts.tag IN ('bills', 'invite')
			and ts.tourn = 1400
		`, {
			type : db.sequelize.QueryTypes.SELECT,
		});

		const results = [];

		existingFiles.forEach( async invite => {

			invite.page_order = 1;
			let filepath = '';

			invite.filename = invite.value;
			delete invite.value;

			if (invite.tag === 'invite') {
				invite.page_order = 1;
				invite.label = 'Tournament Invitation';
				filepath = `tourns/${invite.tourn}/${invite.filename}`;
			} else if (invite.tag === 'bills') {
				invite.page_order = 2;
				invite.label = 'Congress Legislation';
				filepath = `tourns/${invite.tourn}/bills/${invite.filename}`;
			}

			const inviteFile = await db.file.create(invite);
			invite.result = `I have created an invite file ${inviteFile.id} for tourn ${invite.tourn}`;
			results.push(invite);

			const newPath = `tourns/${invite.tourn}/postings/${inviteFile.id}/${inviteFile.filename}`;
			await s3Client.mv(filepath, newPath);
		});

		res.json(results);
	},
};

export default fixInvite;
