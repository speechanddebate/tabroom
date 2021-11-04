// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { listSchools } from '../../../controllers/tourn/register/schools.js';
import { listEvents } from '../../../controllers/tourn/register/events.js';
import { listEntries } from '../../../controllers/tourn/register/entries.js';
import { listJudges } from '../../../controllers/tourn/register/judges.js';

export default [
	{ path : '/tourn/{tourn_id}/register/schools' , module : listSchools } ,
	{ path : '/tourn/{tourn_id}/register/events'  , module : listEvents },
	{ path : '/tourn/{tourn_id}/register/entries' , module : listEntries },
	{ path : '/tourn/{tourn_id}/register/judges'  , module : listJudges },
];
