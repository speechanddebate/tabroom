// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { tournQualifierResult, eventQualifierResult } from '../../../controllers/tourn/results/qualifier.js';

export default [
	{ path : '/tourn/{tourn_id}/result/event/qualifiers' , module : eventQualifierResult } ,
	{ path : '/tourn/{tourn_id}/result/qualifiers'       , module : tournQualifierResult } ,
];
