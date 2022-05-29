// These paths are ones that require a caselist_key
import getPersonChapters from '../../../controllers/caselist/getPersonChapters.js';
import getPersonRounds from '../../../controllers/caselist/getPersonRounds.js';
import getPersonStudents from '../../../controllers/caselist/getPersonStudents.js';
import postCaselistLink from '../../../controllers/caselist/postCaselistLink.js';

export default [
	{ path: '/caselist/chapters', module : getPersonChapters },
	{ path: '/caselist/rounds', module : getPersonRounds },
	{ path: '/caselist/students', module : getPersonStudents },
	{ path: '/caselist/link', module : postCaselistLink },
];
