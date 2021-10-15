import UAParser from 'ua-parser-js';
import {findLocation, findISP} from '../../../helpers/clientInfo.js'

const ipLocation = {

	GET: async (req, res) => {

		const requestIP = req.get('x-forwarded-for');

		if (
			requestIP.startsWith(127)
			|| requestIP.startsWith(192.168)
		) { 

			console.log(req);

			const locationData = await findLocation(req.params.ip_address);
			const ispData = await findISP(req.params.ip_address);
			const userAgent = UAParser(req.get('user-agent'));

			locationData.isp = ispData.isp;

			if (ispData.isp != ispData.organization) { 
				locationData.organization = organizationData.organization;
			}

			locationData.browser = userAgent.browser;
			locationData.device = userAgent.device;
			locationData.os = userAgent.os;
			res.json(locationData);

		} else { 

			res.json({'message': 'This API for internal NSDA use only'});

		}
	}
};

export default ipLocation;
