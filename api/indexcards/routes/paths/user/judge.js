// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Router controllers
import { saveRubric } from '../../../controllers/user/judge/ballot.js';

export default [
	{ path : '/user/judge/{judge_id}/ballot/rubric' , module : saveRubric }              ,
];
