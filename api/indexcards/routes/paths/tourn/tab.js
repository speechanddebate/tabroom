// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { attendance } from '../../../controllers/tourn/tab/status';
import { changeAccess, changeEventAccess } from '../../../controllers/tourn/setup/access';

export default [
	{ path : '/tourn/{tourn_id}/tab/status/update', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/round/{round_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/timeslot/{timeslot_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/setup/access', module : changeAccess },
	{ path : '/tourn/{tourn_id}/tab/setup/eventaccess', module : changeEventAccess },
];
