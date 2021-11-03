// These paths are ones that require a logged in user but are outside the scope
// of tournament administration.  Typically these are registration & user
// account functions.
import getInvite from '../../../controllers/invite/tourn/getInvite.js';

export default [
	{ path: '/index/{webname}', module : getInvite },
	{ path: '/index/id/{tourn_id}', module : getInvite },
];

