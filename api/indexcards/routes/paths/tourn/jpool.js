// Router controllers
import { messageJPool } from '../../../controllers/tourn/section/blast/message.js';
import { eraseJPool, populateStandby } from '../../../controllers/tourn/section/manageJPools.js';

export default [
	{ path : '/tourn/{tourn_id}/jpool/{jpoolId}/message'  , module : messageJPool }    ,
	{ path : '/tourn/{tourn_id}/jpool/{jpoolId}/populate' , module : populateStandby } ,
	{ path : '/tourn/{tourn_id}/jpool/{jpoolId}/erase'    , module : eraseJPool }      ,
];
