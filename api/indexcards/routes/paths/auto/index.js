// These paths are for tournament published data only, and can be seen by any
// users, even if not logged in.

import { autoPublish } from '../../../controllers/auto/publish';

export default [
	{ path : '/auto/publish' , module : autoPublish },
];
