// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { messageTimeslot } from '../../../controllers/tourn/section/blast/message.js';

export default [
	{ path : '/tourn/{tourn_id}/timeslot/{timeslot_id}/message'  , module : messageTimeslot },
];
