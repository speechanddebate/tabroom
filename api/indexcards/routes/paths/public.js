// Utility functions
import status from '../../controllers/utility/status';
import postError from '../../controllers/utility/postError';

export default [
	{ path : '/status', module : status },
	{ path : '/errors' , module  : postError },
	{ path : '/invite/{webname}' , module  : postError },
];
