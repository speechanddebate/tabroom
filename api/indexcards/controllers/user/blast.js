import notify from '../../helpers/pushNotify';
import config from '../../../config/config';

export const pushMessage = {

	POST: async (req, res) => {

		if (req.body.share_key !== config.SHARE_KEY) {
			return res.status(401).json({ message: 'Invalid internal key' });
		}

		const responseJSON = await notify({
			...req.body,
		});

		res.status(200).json(responseJSON);
	},
};

export default pushMessage;
