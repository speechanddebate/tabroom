// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { listSchools } from '../../../controllers/tourn/register/schools';
import { listEvents } from '../../../controllers/tourn/register/events';
import { listEntries } from '../../../controllers/tourn/register/entries';
import { listJudges } from '../../../controllers/tourn/register/judges';

export default [
	{ path : '/tourn/{tourn_id}/register/schools' , module : listSchools } ,
	{ path : '/tourn/{tourn_id}/register/events'  , module : listEvents },
	{ path : '/tourn/{tourn_id}/register/entries' , module : listEntries },
	{ path : '/tourn/{tourn_id}/register/judges'  , module : listJudges },
];
