// Data posting integrations with NAUDL Salesforce
import { getNAUDLStudents } from '../../../controllers/ext/naudl/students.js';
import { testSalesforceConnection } from '../../../controllers/ext/naudl/conntest.js';

export default [
	{ path : '/naudl/students' , module : getNAUDLStudents } ,
	{ path : '/naudl/salesforce' , module : testSalesforceConnection } ,
];
