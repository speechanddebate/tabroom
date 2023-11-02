// Data posting integrations with NAUDL Salesforce
import { syncNAUDLChapters, syncNAUDLStudents, syncExistingNAUDLStudents } from '../../../controllers/ext/naudl/students.js';

export default [
	{ path : '/naudl/chapters/sync' , module : syncNAUDLChapters } ,
	{ path : '/naudl/students/sync' , module : syncNAUDLStudents } ,
	{ path : '/naudl/students/backlog' , module : syncExistingNAUDLStudents } ,
];
