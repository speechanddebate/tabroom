// These paths are ones that require a share_key
import getShare from '../../../controllers/share/getShare';
import postShare from '../../../controllers/share/postShare';

export default [
	{ path: '/share', module: { ...getShare, ...postShare } },
];
