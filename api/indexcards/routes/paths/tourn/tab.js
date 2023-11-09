// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { attendance, schematStatus, eventDashboard, eventStatus, sideCounts } from '../../../controllers/tourn/tab/status.js';
import { changeAccess, changeEventAccess } from '../../../controllers/tourn/setup/access.js';
// import { eventWins, entryWins } from '../../../controllers/tourn/tab/wins.js';
import { natsJudgePool } from '../../../controllers/tourn/tab/judgePools.js';

export default [
	{ path : '/tourn/{tourn_id}/tab/status/update'                , module : attendance }        ,
	{ path : '/tourn/{tourn_id}/tab/status/round/{roundId}'       , module : attendance }        ,
	{ path : '/tourn/{tourn_id}/tab/status/sidecounts/{roundId}'  , module : sideCounts }        ,
	{ path : '/tourn/{tourn_id}/tab/status/schematic/{roundId}'   , module : schematStatus }     ,
	{ path : '/tourn/{tourn_id}/tab/status/timeslot/{timeslotId}' , module : attendance }        ,
	{ path : '/tourn/{tourn_id}/tab/dashboard'                    , module : eventStatus }       ,
	{ path : '/tourn/{tourn_id}/tab/status/dashboard'             , module : eventDashboard }    ,
	{ path : '/tourn/{tourn_id}/tab/setup/access'                 , module : changeAccess }      ,
	{ path : '/tourn/{tourn_id}/tab/setup/eventaccess'            , module : changeEventAccess } ,
	{ path : '/tourn/{tourn_id}/tab/jpool/createNatsPools'        , module : natsJudgePool }     ,
];
// { path : '/tourn/{tourn_id}/tab/entry/wins/:entryId', module: entryWins },
// { path : '/tourn/{tourn_id}/tab/event/wins/:eventId', module: eventWins },
