
// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../controllers/utility/status';

// import getRound from '../controllers/user/getPerson';
// { path : '/user/person/' , module : getPerson },

export default [
    { path : '/t/{tourn_id}/schools', module : status }
];
