// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../controllers/utility/status';

// Router controllers
import { listSchools } from '../../controllers/tourn/entries/schools';
import { listEvents } from '../../controllers/tourn/entries/events';
import { listEntries } from '../../controllers/tourn/entries/entries';
import { listJudges } from '../../controllers/tourn/entries/judges';

import { roundStatus } from '../../controllers/tourn/tabbing/status';
import { timeslotStatus } from '../../controllers/tourn/tabbing/status';

export default [
    { path : '/tourn/{tourn_id}/entry/schools' , module : listSchools } ,
    { path : '/tourn/{tourn_id}/entry/events'  , module : listEvents },
    { path : '/tourn/{tourn_id}/entry/entries' , module : listEntries },
    { path : '/tourn/{tourn_id}/entry/judges'  , module : listJudges },

	{ path : '/tourn/{tourn_id}/tabbing/round/status/{round_id}', module : roundStatus }
	{ path : '/tourn/{tourn_id}/tabbing/timeslot/status/{timeslot_id}', module : timeslotStatus }
];
