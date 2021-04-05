// Utility functions
import status from '../../controllers/utility/status';
import postError from '../../controllers/utility/postError';
import getInviteByWebname from '../../controllers/public/tourn/getInviteByWebname';

export default [
    { path : '/status', module : status },
    { path : '/errors' , module  : postError },
    { path : '/invite/{webname}' , module  : postError },
];
