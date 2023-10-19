// Data posting integrations with NAUDL Salesforce
import { postNAUDLStudents } from '../../../controllers/ext/naudl/students.js';

export default [
	{ path : '/naudl/students' , module : postNAUDLStudents } ,
];
