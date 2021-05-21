// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../../controllers/utility/status';

// Router controllers
import { attendance } from '../../../controllers/tourn/tab/status';

export default [
	{ path : '/tourn/{tourn_id}/tab/status/round/{round_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/timeslot/{timeslot_id}', module : attendance }
];
