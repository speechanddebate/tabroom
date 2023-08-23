// These paths are for tournament published data only, and can be seen by any
// users, even if not logged in.

import { autoPublish } from '../../../controllers/auto/publish';
import { fixInvite } from '../../../helpers/fixinvite';

export default [
	{ path : '/auto/publish' , module : autoPublish },
	{ path : '/auto/invites' , module : fixInvite },
];
