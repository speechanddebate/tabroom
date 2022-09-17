// import { showDateTime } from '../../../helpers/common';

export const backupTourn = {
	GET: async (req, res) => {
		const db = req.db;
		const tournId = req.params.tourn_id;

		const tourn = await db.tourn.findByPk(tournId);
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
