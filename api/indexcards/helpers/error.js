import { errorLogger } from './logger';
import adminBlast  from './mail';
import config from '../../config/config';

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
			subject : `JS Bug Tripped at ${new Date()}`,
			text    : `Javascript has errors too, and here's one for you!  YAY!
	Stack ${err.stack}

    Full Error ${JSON.stringify(err, null, 4)}

    Session ${JSON.stringify(req.session, null, 4)}

    Parameters ${JSON.stringify(req.params, null, 4)}

    Body ${JSON.stringify(req.body, null, 4)} `,
		};

		try {
			const info = adminBlast(messageData);
			errorLogger.info(info);
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
