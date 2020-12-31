// Utility functions
import status from '../controllers/utility/status';
import postError from '../controllers/utility/postError';

// import getPerson from '../controllers/user/getPerson';
// import getLogin from '../controllers/user/getLogin';
// { path : '/user/person/' , module : getPerson },

export default [
    { path : '/status', module : status },
    { path : '/error' , module  : postError },
];
