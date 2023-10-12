import stacktrace from 'stacktrace-js';
// import { postError } from '../api/api';
// import store from '../redux/store';

const log = (...args) => {
	// Don't log if testing (code is run under node, not in the browser)
	if (typeof process !== 'undefined' && process.env && process.env.NODE_ENV === 'test') {
		return false;
	}

	let err;

	// If only one param, assume it's an error object
	if (args.length === 1) {
		err = args[0];
	} else {
		// Error object is passed as the 5th param by window.onerror
		// fallback to a new error with the msg param for older browsers
		err = args[4] ? args[4] : new Error(args[0]);
	}

	// If the supplied error has a stack trace, use it, otherwise generate one
	err.stack = err.stack || stacktrace.getSync();

	// If debugging in an environment that supports console, pass through to console.log
	if (process.env.REACT_APP_DEBUG) {
		if (typeof window !== 'undefined' && window.console) {
			console.log(args);
		}
	}

	// If remote debugging turned on, send error info
	// Have to convert the Error object to a regular object to stringify it
	// if (process.env.REACT_APP_DEBUG_REMOTE) {
	// 	const body = {
	// 		err: {
	// 			message: err.message,
	// 			stack: err.stack,
	// 		},
	// 		// store: store.getState(),
	// 		userAgent: navigator ? navigator.userAgent : null,
	// 	};

	// 	postError(body);
	// }
};
export default log;

export const onerror = (msg, url, line, column, error) => {
	// Pass off to the custom logger
	log(msg, url, line, column, error);

	// Allows the default handler to also trigger
	return false;
};
