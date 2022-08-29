// These paths are ones that require a caselist_key
import getPersonChapters from '../../../controllers/caselist/getPersonChapters';
import getPersonRounds from '../../../controllers/caselist/getPersonRounds';
import getPersonStudents from '../../../controllers/caselist/getPersonStudents';
import postCaselistLink from '../../../controllers/caselist/postCaselistLink';

export default [
	{ path: '/caselist/chapters', module : getPersonChapters },
	{ path: '/caselist/rounds', module : getPersonRounds },
	{ path: '/caselist/students', module : getPersonStudents },
	{ path: '/caselist/link', module : postCaselistLink },
];
