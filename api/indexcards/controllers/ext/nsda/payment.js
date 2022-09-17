// Functions to establish access parameters
import hmacSHA512 from 'crypto-js/hmac-sha512';
import Base64 from 'crypto-js/enc-base64';
import config from '../../../../config/config';

export const postPayment = {
	GET: async (req, res) => {
		const db = req.db;
		const tourn = await db.summon(db.tourn, req.params.tourn_id);

		if (!tourn.settings && !tourn.settings.store_cards) {
			return res.status(400).json({ message: 'No shopping cart applies to that tournament' });
		}

		res.status(200).json(tourn);
	},
	POST: async (req, res) => {
		const db = req.db;
		const postRequest = req.body;

		if (!postRequest.invoice_id) {
			return res.status(400).json({ message: 'Invalid request sent: no invoice ID' });
		}

		const hashDigest = Base64.stringify(hmacSHA512(postRequest.invoice_id, config.BILLING_KEY));

		if (hashDigest !== postRequest.hashKey) {
			return res.status(400).json({ message: `Permission key invalid` });
		}

		if (!postRequest.tourn_id) {
			return res.status(400).json({ message: 'Invalid request sent: no tournament ID' });
		}

		const tourn = await db.summon(db.tourn, postRequest.tourn_id);
		const [invoiceId, cartKey] = postRequest.invoice_id.split('-');

		if (!tourn.settings.store_carts) {
			return res.status(400).json({ message: 'No shopping cart found for that tournament' });
		}

		if (!tourn.settings.store_carts[cartKey]) {
			return res.status(400).json({ message: `Invoice ${invoiceId} cart ${cartKey} not found` });
		}
		res.status(200).json(tourn);
	},
};

export default postPayment;
