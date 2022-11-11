// These paths are ones that require a share_key
import postShare from '../../../controllers/ext/share/postShare';

export default [
	{ path: '/share', module: { ...postShare } },
];
