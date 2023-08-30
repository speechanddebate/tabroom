// Integrations with NSDA gateways, payments and services that require share keys

// Router controllers
import getPersonHistory from '../../../controllers/ext/nsda/getPersonHistory';
import { postPayment } from '../../../controllers/ext/nsda/payment';
import { syncNatsAppearances } from '../../../controllers/ext/nsda/natsAppearances';

export default [
	{ path : '/nsda/history'            , module : getPersonHistory },
	{ path : '/nsda/payment'            , module : postPayment },
	{ path : '/nsda/payment/{tourn_id}' , module : postPayment },
	{ path : '/nsda/nats-appearances'   , module : syncNatsAppearances },
];
