import axios from 'axios';
import config from '../../config/config';

export const sendToSalesforce = async (body, url) => {

	const naudl = config.NAUDL;

	const authClient = `grant_type=password&client_id=${naudl.CLIENT_ID}&client_secret=${naudl.CLIENT_SECRET}`;
	const authUser = `&username=${naudl.USERNAME}&password=${naudl.PW}`;
	const authResponse = await axios.post(
		`https://login.salesforce.com/services/oauth2/token?${authClient}${authUser}`,
	);

	console.log(authResponse.data);

	if (authResponse?.data) {
		const postResponse = await axios.post(
			`${authResponse.data.instance_url}${url}`,
			body,
			{
				headers : {
					Authorization  : `OAuth ${authResponse.data.access_token}`,
					'Content-Type' : 'application/json',
					Accept         : 'application/json',
				},
			}
		);
		return postResponse;
	}

	return 'Authentication Failure';

};

export default sendToSalesforce;
