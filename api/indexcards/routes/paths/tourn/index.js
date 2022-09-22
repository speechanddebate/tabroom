// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import data from './data';
import register from './register';
import section from './section';
import tab from './tab';

export default [
	...data,
	...register,
	...section,
	...tab,
];
