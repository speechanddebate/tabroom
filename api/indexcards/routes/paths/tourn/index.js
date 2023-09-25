// These paths are particular to tournament administration, and so require a
// logged in user with access to the tournament in question.

// Utility functions
import data from './data';
import register from './register';
import section from './section';
import round from './round';
import tab from './tab';
import jpool from './jpool';
import rpool from './rpool';
import result from './result';
import timeslot from './timeslot';

export default [
	...data,
	...register,
	...section,
	...round,
	...tab,
	...jpool,
	...rpool,
	...result,
	...timeslot,
];
