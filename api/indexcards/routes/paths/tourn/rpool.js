// Router controllers
import { addRoundToRPool } from '../../../controllers/tourn/section/manageRPools.js';

export default [
	{ path : '/tourn/{tourn_id}/rpool/{rpoolId}/addRound' , module : addRoundToRPool },
];
