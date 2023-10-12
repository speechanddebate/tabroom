// Data posting integrations with NAUDL Salesforce
import { getNAUDLStudents } from '../../../controllers/ext/naudl/students.js';

export default [
	{ path : '/naudl/students' , module : getNAUDLStudents } ,
];
