import axios from 'axios';
import config from '../../config/config';

export const sendToSalesforce = async (body, url) => {

	const authData = await authSalesforce();
	console.log(authData);

	if (authData) {
		const postResponse = await axios.post(
			`${authData.instance_url}${url}`,
			body,
			{
				headers : {
					Authorization  : `OAuth ${authData.access_token}`,
					'Content-Type' : 'application/json',
					Accept         : 'application/json',
				},
			}
		);
		return postResponse;
	}

	return 'Authentication Failure';
};

export const getSalesforceTeams = async () => {

	const authData = await authSalesforce();

	const queryBase = `${authData.instance_url}/services/data/v59.0/query`;
	const queryText = `?q=SELECT+Tabroom_teamid__c+FROM+School__c+WHERE+Tabroom_teamid__c+!=+null`;

	const getResponse = await axios.get(
		`${queryBase}${queryText}`,
		{
			headers : {
				Authorization  : `OAuth ${authData.access_token}`,
				'Content-Type' : 'application/json',
				Accept         : 'application/json',
			},
		}
	);

	return getResponse.data;
};

const authSalesforce = async () => {

	const naudl = config.NAUDL;

	const authClient = `grant_type=password&client_id=${naudl.CLIENT_ID}&client_secret=${naudl.CLIENT_SECRET}`;
	const authUser = `&username=${naudl.USERNAME}&password=${naudl.PW}`;
	const authResponse = await axios.post(
		`https://login.salesforce.com/services/oauth2/token?${authClient}${authUser}`,
	);

	console.log(authResponse.data);

	if (authResponse && authResponse.data) {
		return authResponse.data;
	}

	return false;
};

export default sendToSalesforce;
