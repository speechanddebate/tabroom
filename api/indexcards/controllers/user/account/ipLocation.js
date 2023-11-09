import UAParser from 'ua-parser-js';
import { findLocation, findISP } from '../../../helpers/clientInfo';

const ipLocation = {
	GET: async (req, res) => {
		const requestIP = req.get('x-forwarded-for') || req.ip;

		if (
			requestIP?.startsWith(127)
			|| requestIP?.startsWith(192.168)
			|| requestIP?.startsWith(10.19)
			|| process.env.NODE_ENV === 'development'
			|| process.env.NODE_ENV === 'test'
		) {
			const locationData = await findLocation(req.params.ip_address);
			const ispData = await findISP(req.params.ip_address);
			const userAgent = UAParser(req.get('user-agent'));

			locationData.isp = ispData.isp;

			if (ispData.isp !== ispData.organization) {
				locationData.organization = ispData.organization;
			}

			locationData.browser = userAgent.browser;
			locationData.device = userAgent.device;
			locationData.os = userAgent.os;
			res.json(locationData);

		} else {
			res.json( { message: 'This API for internal NSDA use only' } );
		}
	},
};

export default ipLocation;
