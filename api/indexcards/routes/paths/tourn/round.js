// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { roundJudges } from '../../../controllers/tourn/section/cleanJudges';
import { messageRound } from '../../../controllers/tourn/section/blast/message.js';
import { blastRound } from '../../../controllers/tourn/section/blast/pairing.js';
import { getRoundLog } from '../../../controllers/tourn/schematic/changeLog.js';

export default [
	{ path : '/tourn/{tourn_id}/round/{round_id}/judges'  , module : roundJudges }  ,
	{ path : '/tourn/{tourn_id}/round/{round_id}/message' , module : messageRound } ,
	{ path : '/tourn/{tourn_id}/round/{round_id}/blast'   , module : blastRound }   ,
	{ path : '/tourn/{tourn_id}/round/{round_id}/log'     , module : getRoundLog }  ,
];
