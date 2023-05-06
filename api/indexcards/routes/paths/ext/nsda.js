// Integrations with NSDA gateways, payments and services that require share keys

// Router controllers
import { postPayment } from '../../../controllers/ext/nsda/payment';
import { syncNatsAppearances } from '../../../controllers/ext/nsda/natsAppearances';

export default [
	{ path : '/nsda/payment'            , module : postPayment },
	{ path : '/nsda/payment/{tourn_id}' , module : postPayment },
	{ path : '/nsda/nats-appearances'   , module : syncNatsAppearances },
];
