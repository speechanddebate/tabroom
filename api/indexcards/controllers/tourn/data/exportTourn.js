// import { showDateTime } from '../../../helpers/common';

export const backupTourn = {
	GET: async (req, res) => {
		const db = req.db;

		const tournShell  = await db.tourn.findByPk(req.params.tourn_id);
		const tourn = tournShell.dataValues;

		// but her emails
		tourn.emails      = await db.email.findAll({ where: { tourn: tourn.id } });
		tourn.permissions = await db.permission.findAll({ where: { tourn: tourn.id } });
		tourn.webpages    = await db.webpage.findAll({ where: { tourn: tourn.id } });
		tourn.settings    = await db.tournSetting.findAll({ where: { tourn: tourn.id } });

		// Room Pools
		// Sites
		// Circuits

		return res.status(200).json(tourn);
	},
};

export const restoreTourn = {
	POST: async (req, res) => {

		const tournData = req.body;

		if (!tournData.tourn_id) {
			tournData.tourn_id = req.params.tourn_id;
		}

		if (!tournData.tourn_id) {
			tournData.tourn_id = req.session.tourn_id;
		}

		// process it later but for now

		return res.status(200).json({ message: 'yay' });
	},
};
