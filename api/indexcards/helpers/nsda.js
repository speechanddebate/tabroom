import fetch from 'node-fetch';
import CryptoJS from 'crypto-js';
import config from '../../config/config';

export const getNSDA  = async (path) => {

	const uri = `${config.NSDA_API_ENDPOINT}${config.NSDA_API_PATH}${path}`;
	const words = CryptoJS.enc.Utf8.parse(`${config.NSDA_API_USER}:${config.NSDA_API_KEY}`);
	const authToken = CryptoJS.enc.Base64.stringify(words);

	try {
		const response = await fetch(uri, {
			method  : 'get',
			headers : { Authorization : `Basic ${authToken}` },
		});

		return await response.json();

	} catch (err) {
		return {
			error: true,
			message : `Error caught on fetch: ${err}`,
		};
	}
};

export default getNSDA;
