
// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import status from '../../controllers/utility/status';
import getSchools from '../../controllers/tourn/entries/getSchools';

// import getRound from '../controllers/user/getPerson';
// { path : '/user/person/' , module : getPerson },

export default [
    { path : '/tourn/{tourn_id}/schools', module : getSchools }
];
