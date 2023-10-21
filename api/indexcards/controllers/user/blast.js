import notify from '../../helpers/pushNotify';
import config from '../../../config/config';

export const pushMessage = {

	POST: async (req, res) => {

		if (!req.session?.site_admin
			&& req.body.share_key !== config.SHARE_KEY
		) {

			console.log(req.body);
			console.log(req.body.share_key);
			console.log(config.SHARE_KEY);
			return res.status(401).json({ message: 'Invalid internal key' });
		}

		const responseJSON = await notify({
			...req.body,
		});

		res.status(200).json(responseJSON);
	},
};

export default pushMessage;
