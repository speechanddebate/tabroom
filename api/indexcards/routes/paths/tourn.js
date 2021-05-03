// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../controllers/utility/status';

// Router controllers
import { listSchools } from '../../controllers/tourn/entries/schools';
import { listEvents } from '../../controllers/tourn/entries/events';
import { listEntries } from '../../controllers/tourn/entries/entries';
import { listJudges } from '../../controllers/tourn/entries/judges';

export default [
    { path : '/tourn/{tourn_id}/schools' , module : listSchools } ,
    { path : '/tourn/{tourn_id}/events'  , module : listEvents },
    { path : '/tourn/{tourn_id}/entries' , module : listEntries },
    { path : '/tourn/{tourn_id}/judges'  , module : listJudges },
];
