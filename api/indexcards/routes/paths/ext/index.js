// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import caselist from './caselist';
import share from './share';
import nsda from './nsda';

export default [
	...caselist,
	...share,
	...nsda,
];
