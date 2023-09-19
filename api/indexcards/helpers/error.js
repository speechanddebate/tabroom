import { errorLogger } from './logger';
import { adminBlast } from './mail';
import config from '../../config/config';

const errorHandler = async (err, req, res, next) => {

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
				env,
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

	// Production bugs should find their way to Palmer
	const env = process.env.NODE_ENV || 'development';

	if (env === 'production') {

		const messageData = {
			from    : 'error-handler@tabroom.com',
			email   : config.ERROR_DESTINATION,
			subject : `Indexcards Bug Tripped`,
			text    : `
Stack
${err.stack}

Login Session 
${JSON.stringify(req.session, null, 4)}

Request Parameters
${JSON.stringify(req.params, null, 4)}

Request Body
${JSON.stringify(req.body, null, 4)}

Raw Full Error Object
${JSON.stringify(err, Object.getOwnPropertyNames(err))}`,

		};

		try {
			adminBlast(messageData);
			err.message += ` Also, this stack was emailed to the admins to ${config.ERROR_DESTINATION}`;
		} catch (error) {
			errorLogger.info(error);
			err.message += ` Also, error response on sending email: ${err}`;
		}
	}

	return res.status(500).json({
		message          : err.message || 'Internal server error',
		logCorrelationId : req.uuid,
		path             : req.path,
		stack            : err.stack,
		env,
	});
};

export default errorHandler;
