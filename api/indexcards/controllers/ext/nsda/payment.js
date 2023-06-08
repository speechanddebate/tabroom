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

		const hashDigest = Base64.stringify(hmacSHA512(postRequest.invoice_id, config.NSDA_API_KEY));

		if (hashDigest !== postRequest.hash_key) {
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

		const tournCart = tourn.settings.store_carts[cartKey];
		res.status(201).json(tourn);

		const now = new Date();

		tournCart.tabroom   = postRequest.items[config.NSDA_PRODUCT_CODES.tabroom];
		tournCart.nc        = postRequest.items[config.NSDA_PRODUCT_CODES.campus];
		tournCart.nco       = postRequest.items[config.NSDA_PRODUCT_CODES.campus_observers];
		tournCart.paid      = 1;
		tournCart.paid_at   = now;

		tourn.settings.store_carts[cartKey] = tournCart;

		const totals = {
			nc      : 0,
			tabroom : 0,
			nco     : 0,
		};

		Object.entries(tourn.settings.store_carts).forEach( ([req, cart]) => {
			totals.tabroom += cart.tabroom;
			totals.nco += cart.nco;
			totals.nc += cart.nc;
		});

		await db.setting(tourn, 'tabroom_purchased', totals.tabroom);
		await db.setting(tourn, 'nc_purchased', totals.nc);
		await db.setting(tourn, 'nco_purchased', totals.nco);
		await db.setting(tourn, 'store_carts', { json: tourn.settings.store_carts[cartKey] });

		res.status(201).json({ error: false, message: `Invoice ${postRequest.invoice_id} marked as paid` });
	},
};

export default postPayment;
