// Data posting integrations with NAUDL Salesforce
import { postNAUDLStudents, getNAUDLChapters } from '../../../controllers/ext/naudl/students.js';

export default [
	{ path : '/naudl/students' , module : postNAUDLStudents } ,
	{ path : '/naudl/chapters' , module : getNAUDLChapters } ,
];
