import notify from '../../helpers/pushNotify';
import config from '../../../config/config';
import { errorLogger } from '../../helpers/logger';

export const pushMessage = {

	POST: async (req, res) => {

		if (req.body.share_key !== config.SHARE_KEY) {
			errorLogger.error('Invalid internal key, blast not sent');
			return res.status(401).json({ message: 'Invalid internal key' });
		}

		errorLogger.info(req.body);
		const responseJSON = await notify({
			...req.body,
		});

		res.status(200).json(responseJSON);
	},
};

export default pushMessage;
