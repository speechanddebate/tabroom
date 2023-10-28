import notify from '../../helpers/pushNotify';
import config from '../../../config/config';
import { debugLogger } from '../../helpers/logger';

export const pushMessage = {

	POST: async (req, res) => {

		debugLogger(req.body);
		console.log(req.body);

		if (req.body.share_key !== config.SHARE_KEY) {
			debugLogger('Invalid internal key, blast not sent');
			console.log('does this get caught');
			return res.status(401).json({ message: 'Invalid internal key' });
		}

		const responseJSON = await notify({
			...req.body,
		});

		res.status(200).json(responseJSON);
	},
};

export default pushMessage;
