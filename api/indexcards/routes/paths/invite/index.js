// These paths are for tournament public data only

import { getInvite, getRounds } from '../../../controllers/public/invite/tournInvite';
import { futureTourns } from '../../../controllers/public/invite/tournList';

export default [
	{ path: '/invite/{webname}', module : getInvite },
	{ path: '/invite/round/{round_id}', module : getRounds },
	{ path: '/invite/upcoming', module : futureTourns },
	{ path: '/invite/upcoming/:circuit', module : futureTourns },
];
