// These paths are for tournament public data only

import { getInvite, getRounds } from '../../../controllers/invite/tourn/getInvite';

export default [
	{ path: '/invite/{webname}', module : getInvite },
	{ path: '/invite/round/{round_id}', module : getRounds },
];
