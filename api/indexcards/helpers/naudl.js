import jsforce from 'jsforce';
import config from '../../config/config';

export const sendToSalesforce = (body, url) => {

	const conn = new jsforce.Connection();

	const _request = {
		url: config.NAUDL.URL + url,
		body,
		method: 'post',
		headers : {
			'Content-Type' : 'application/json',
		},
	};

	const sendData = (err) => {
		if (err) {
			return console.error(err);
		}

		conn.request(_request, (err, response) => {
			return response;
		});
	};

	const response = conn.login(
		config.NAUDL.USERNAME,
		config.NAUDL.PW + config.NAUDL.TOKEN,
		sendData
	);

	return response;
};

export default sendToSalesforce;
