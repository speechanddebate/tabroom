// This piece of code blatantly stolen from Hardy & the NSDA API project.
// --CLP
import { errorLogger } from './logger';

const errorHandler = (err, req, res, next) => {

	// Delegate to default express error handler if headers are already sent
	if (res.headersSent) {
		return next(err);
	}

	// Validation error object from OpenAPI
	if (err.status || err.errors) {
		if (err.status === 400) {
			return res.status(err.status).json({
				message          : 'Validation error',
				errors           : err.errors,
				stack            : err.stack,
				logCorrelationId : req.uuid,
			});
		}
		if (err.status === 401) {
			return res.status(err.status).json({
				message: err.message,
			});
		}
	}

	// Default to a 500 error and give me a stack trace PLEASE ALWAYS GIVE ME A
	// FRIGGIN STACK TRACE WHY IS THIS NOT THE DEFAULT DEV BEHAVIOR OMFG.
	errorLogger.error(err, err.stack);

	return res.status(500).json({
		message          : err.message || 'Internal server error',
		logCorrelationId : req.uuid,
		path             : req.path,
		stack            : err.stack,
	});
};

export default errorHandler;
