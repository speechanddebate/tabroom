import winston from 'winston';

export const debugLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	silent: process.env.NODE_ENV === 'test',
	transports: [
		new winston.transports.Console(),
		new winston.transports.File({
			filename: './logs/debug.log',
		}),
	],
});

export const requestLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	silent: process.env.NODE_ENV === 'test',
	transports: [
		new winston.transports.Console(),
		new winston.transports.File({
			filename: './logs/request.log',
		}),
	],
});

export const errorLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	silent: process.env.NODE_ENV === 'test',
	transports: [
		new winston.transports.Console(),
		new winston.transports.File({
			filename: './logs/error.log',
		}),
	],
});

export const clientLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	silent: process.env.NODE_ENV === 'test',
	transports: [
		new winston.transports.Console(),
		new winston.transports.File({
			filename: './logs/client.log',
		}),
	],
});

export const queryLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	// silent: process.env.NODE_ENV === 'test',
	transports: [
		new winston.transports.Console(),
	],
});

export const autoemailLogger = winston.createLogger({
	level: 'debug',
	format: winston.format.combine(
		winston.format.timestamp({ format: 'YYYY-MM-DD HH:mm:ss' }),
		winston.format.json(),
	),
	exitOnError: false,
	transports: [
		new winston.transports.Console(),
		new winston.transports.File({
			filename: './logs/autoemail.log',
		}),
	],
});
