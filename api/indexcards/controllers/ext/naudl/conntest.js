import axios from 'axios';
import config from '../../../../config/config';

export const testSalesforceConnection = {
	GET: async (req, res) => {

		const auth = async () => {

			const naudl = config.NAUDL;
			console.log(naudl);

			try {

				const authClient = `grant_type=password&client_id=${naudl.CLIENT_ID}&client_secret=${naudl.CLIENT_SECRET}`;
				const authUser = `&username=${naudl.USERNAME}&password=${naudl.PW}`;

				const response = await axios.post(
					`https://login.salesforce.com/services/oauth2/token?${authClient}${authUser}`,
				);
				return response?.data ? response.data : undefined;
			} catch (err) {
				console.error(err);
				return undefined;
			}
		};

		const response = await auth();

		if (response) {
			res.status(200).json(response);
		}
	},
};

export default testSalesforceConnection;
