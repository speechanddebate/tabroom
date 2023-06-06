// Router controllers
import { messageJPool } from '../../../controllers/tourn/section/blast/message.js';

export default [
	{ path : '/tourn/{tourn_id}/jpool/{jpool_id}/message' , module : messageJPool },
];
