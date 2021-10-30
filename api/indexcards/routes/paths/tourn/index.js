// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import tab from './tab.js';
import register from './register.js';

export default [
	...tab,
	...register,
];
