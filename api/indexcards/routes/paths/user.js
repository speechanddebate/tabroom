// These paths are ones that require a logged in user but are outside the scope
// of tournament administration.  Typically these are registration & user
// account functions.

// Utility functions
import status from '../../controllers/utility/status';
import postError from '../../controllers/utility/postError';

// import getPerson from '../controllers/user/getPerson';
// import getLogin from '../controllers/user/getLogin';
// { path : '/user/person/' , module : getPerson },

export default [
    { path : '/profile', module : status },
];
