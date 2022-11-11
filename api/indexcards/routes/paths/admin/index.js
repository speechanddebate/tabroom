// These paths are for tournament public data only

import { getInvite, getRounds } from '../../../controllers/public/invite/tournInvite';

export default [
	{ path: '/invite/{webname}', module : getInvite },
	{ path: '/invite/round/{round_id}', module : getRounds },
];
