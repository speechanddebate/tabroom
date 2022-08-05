// These paths are ones that require a share_key
import getShare from '../../../controllers/share/getShare.js';
import postShare from '../../../controllers/share/postShare.js';

export default [
	{ path: '/share', module: { ...getShare, ...postShare } },
];
