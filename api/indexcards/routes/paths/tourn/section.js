// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { sectionRobin } from '../../../controllers/tourn/section/sectionRobin.js';
import { panelCleanJudges } from '../../../controllers/tourn/section/cleanJudges';
import { messageSection } from '../../../controllers/tourn/section/blast/message.js';
import { blastSection } from '../../../controllers/tourn/section/blast/pairing.js';

export default [
	{ path : '/tourn/{tourn_id}/section/robin/{eventId}'          , module : sectionRobin }     ,
	{ path : '/tourn/{tourn_id}/section/judges/{sectionId}/clean' , module : panelCleanJudges } ,
	{ path : '/tourn/{tourn_id}/section/{sectionId}/message'      , module : messageSection }   ,
	{ path : '/tourn/{tourn_id}/section/{sectionId}/blast'        , module : blastSection }     ,
];
