// Router controllers
import { messageJPool } from '../../../controllers/tourn/section/blast/message.js';
import { populateStandby } from '../../../controllers/tourn/section/populateStandby.js';

export default [
	{ path : '/tourn/{tourn_id}/jpool/{jpool_id}/message' , module : messageJPool },
	{ path : '/tourn/{tourn_id}/jpool/{jpool_id}/populate' , module : populateStandby },
];
