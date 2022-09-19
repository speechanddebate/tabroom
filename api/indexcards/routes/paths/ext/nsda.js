// Integrations with NSDA gateways, payments and services that require share keys

// Router controllers
import { postPayment } from '../../../controllers/ext/nsda/payment';

export default [
	{ path : '/nsda/payment', module : postPayment },
	{ path : '/nsda/payment/{tourn_id}', module : postPayment },
];
