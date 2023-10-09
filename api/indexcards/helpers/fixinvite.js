/* eslint-disable camelcase */
import db from './db.js';
import s3Client from './aws.js';

export const fixInvite = {
	GET: async (req, res) => {

		const existingFiles = await db.sequelize.query(`
			select ts.tourn, ts.tag, ts.value, ts.timestamp
				from tourn_setting ts
			where ts.tag IN ('bills', 'invite')
				and not exists (
					select file.id
					from file
					where file.tag IN ('invite', 'bills')
					and file.tourn = ts.tourn
				)
				order by ts.tourn DESC
		`, {
			type : db.sequelize.QueryTypes.SELECT,
		});

		console.log(`I have found ${existingFiles.length} files`);

		const promises = existingFiles.map( async (invite) => {

			let oldPath = '';

			invite.page_order = 1;
			invite.filename = invite.value;
			invite.published = 1;
			invite.uploaded = invite.timestamp;
			invite.type = 'front';

			delete invite.value;

			if (invite.tag === 'invite') {
				invite.label = 'Tournament Invitation';
				oldPath = `tourns/${invite.tourn}/${invite.filename}`;
			} else if (invite.tag === 'bills') {
				invite.page_order = 2;
				invite.label = 'Congress Legislation';
				oldPath = `tourns/${invite.tourn}/bills/${invite.filename}`;
			}

			const inviteFile = await db.file.create(invite);
			invite.result = `I have created an invite file ${inviteFile.id} for tourn ${invite.tourn}`;
			const newPath = `tourns/${invite.tourn}/postings/${inviteFile.id}/${inviteFile.filename}`;

			try {
				invite.aws = await s3Client.cp(oldPath, newPath);
			} catch (err) {
				invite.err = err;
			}

			return invite;
		});

		const results = await Promise.all(promises);
		res.status(200).json(results);
	},
};

export default fixInvite;
