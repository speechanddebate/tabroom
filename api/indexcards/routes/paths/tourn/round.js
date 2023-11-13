// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { roundJudges } from '../../../controllers/tourn/section/cleanJudges';
import { messageRound } from '../../../controllers/tourn/section/blast/message.js';
import { blastRound } from '../../../controllers/tourn/section/blast/pairing.js';
import { getRoundLog } from '../../../controllers/tourn/schematic/changeLog.js';
import { mergeTimeslotRounds, unmergeTimeslotRounds } from '../../../controllers/tourn/section/mergeRounds.js';

export default [
	{ path : '/tourn/{tourn_id}/round/{roundId}/judges'  , module : roundJudges }           ,
	{ path : '/tourn/{tourn_id}/round/{roundId}/message' , module : messageRound }          ,
	{ path : '/tourn/{tourn_id}/round/{roundId}/blast'   , module : blastRound }            ,
	{ path : '/tourn/{tourn_id}/round/{roundId}/log'     , module : getRoundLog }           ,
	{ path : '/tourn/{tourn_id}/round/{roundId}/merge'   , module : mergeTimeslotRounds }   ,
	{ path : '/tourn/{tourn_id}/round/{roundId}/unmerge' , module : unmergeTimeslotRounds } ,
];
