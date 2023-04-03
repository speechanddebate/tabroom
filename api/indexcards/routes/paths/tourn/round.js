// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { roundJudges } from '../../../controllers/tourn/section/cleanJudges';
import { messageRound } from '../../../controllers/tourn/section/blast/message.js';

export default [
	{ path : '/tourn/{tourn_id}/round/{round_id}/judges'  , module : roundJudges },
	{ path : '/tourn/{tourn_id}/round/{round_id}/message' , module : messageRound },
];
