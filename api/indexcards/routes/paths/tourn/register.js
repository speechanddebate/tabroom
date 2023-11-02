// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { listSchools } from '../../../controllers/tourn/register/schools';
import { listEvents } from '../../../controllers/tourn/register/events';
import { listEntries } from '../../../controllers/tourn/register/entries';
import { listJudges, getActiveJudges } from '../../../controllers/tourn/register/judges';
import { searchAttendees } from '../../../controllers/tourn/register/search';

export default [
	{ path : '/tourn/{tourn_id}/register/schools'                     , module : listSchools }     ,
	{ path : '/tourn/{tourn_id}/register/events'                      , module : listEvents }      ,
	{ path : '/tourn/{tourn_id}/register/entries'                     , module : listEntries }     ,
	{ path : '/tourn/{tourn_id}/register/judges'                      , module : listJudges }      ,
	{ path : '/tourn/{tourn_id}/register/judges/{category_id}/active' , module : getActiveJudges } ,
	{ path : '/tourn/{tourn_id}/register/search/:searchString'        , module : searchAttendees } ,
];
