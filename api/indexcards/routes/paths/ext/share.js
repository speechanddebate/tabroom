// These paths are ones that require a share_key
import getShare from '../../../controllers/ext/share/getShare';
import postShare from '../../../controllers/ext/share/postShare';

export default [
	{ path: '/share', module: { ...getShare, ...postShare } },
];
