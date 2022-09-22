// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { sectionRobin } from '../../../controllers/tourn/section/sectionRobin.js';

export default [
	{ path : '/tourn/{tourn_id}/section/robin/{event_id}', module : sectionRobin },
];
