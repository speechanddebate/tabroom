// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { attendance, eventStatus } from '../../../controllers/tourn/tab/status';
import { changeAccess, changeEventAccess } from '../../../controllers/tourn/setup/access';
import { eventWins, entryWins } from '../../../controllers/tourn/tab/wins';

export default [
	{ path : '/tourn/{tourn_id}/tab/status/update', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/round/{round_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/timeslot/{timeslot_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/dashboard', module : eventStatus },
	{ path : '/tourn/{tourn_id}/tab/setup/access', module : changeAccess },
	{ path : '/tourn/{tourn_id}/tab/setup/eventaccess', module : changeEventAccess },
	{ path : '/tourn/{tourn_id}/tab/entry/wins/:entryId', module: entryWins },
	{ path : '/tourn/{tourn_id}/tab/event/wins/:eventId', module: eventWins },
];