// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../controllers/utility/status';

// Router controllers
import { listSchools } from '../../controllers/tourn/register/schools';
import { listEvents } from '../../controllers/tourn/register/events';
import { listEntries } from '../../controllers/tourn/register/entries';
import { listJudges } from '../../controllers/tourn/register/judges';

import { attendance } from '../../controllers/tourn/tab/status';

export default [
    { path : '/tourn/{tourn_id}/register/schools' , module : listSchools } ,
    { path : '/tourn/{tourn_id}/register/events'  , module : listEvents },
    { path : '/tourn/{tourn_id}/register/entries' , module : listEntries },
    { path : '/tourn/{tourn_id}/register/judges'  , module : listJudges },

	{ path : '/tourn/{tourn_id}/tab/status/round/{round_id}', module : attendance },
	{ path : '/tourn/{tourn_id}/tab/status/timeslot/{timeslot_id}', module : attendance }
];
