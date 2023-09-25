// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { qualifierResult } from '../../../controllers/tourn/results/qualifier.js';

export default [
	{ path : '/tourn/{tourn_id}/event/{event_id}/qualifiers'  , module : qualifierResult }  ,
];
