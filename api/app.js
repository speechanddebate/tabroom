// Express is the router and framework
const express = require('express');
const path    = require('path');

// Env determines if we're running production, development or whatever
const env = process.env.NODE_ENV || "development";

// Conflict just parses the configuration file for the environment in question
const config = require(__dirname + '/config/config.json')[env];

// Morgan is a logging engine that displays to console in dev and logfiles in prod.
const morgan = require('morgan');

// CookieParser reads in the data from cookies for auth
const cookieParser = require('cookie-parser');

// BodyParser allows one to access the data from POSTs
const bodyParser = require('body-parser');

// Crypt allows the API to parse the tabroom native cookie format for auth
const crypt = require('crypt3');

var app = express();

// Log requests to the console on dev, into a file on prod Note that this must
// come before you handle the routes (app.use('/')) or else it logs nothing.
// Palmer's mistakes become your wisdom. 

if (env === 'development') {
	app.use(morgan('combined'));
} else { 
	var accessLogStream = rfs('tabroom-api.log',
		{
			path     : '/var/log/tabroom/tabroom-api.log',
			interval : '3d', // rotate every 3 days
			flags    : 'a'
		}
	);
	app.use(morgan('combined', { stream: accessLogStream }));
}

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

app.use(cookieParser());

// Check for a valid session and populate to the req.session
var authenticate = require('./lib/user/authenticate');

app.use(function(req, res, next) { 
	authenticate(req,res,next);
});

// Set the URL routes
app.use('/', require('./routes'));

// Catch 404s and forward to error handler
app.use(function(req, res, next) {
	var err = new Error('Not Found');
	err.status = 404;
	next(err);
});

// Development error handler, prints stacktrace
if (env === 'development') {

	app.use(function(err, req, res, next) {
		res.status(err.status || 500);
		res.json({
			message: err.message,
			error: err
		});
	});

// Production error handler, No stacktraces leaked to user
} else { 
	app.use(function(err, req, res, next) {
		res.status(err.status || 500);
		res.json({
			message: err.message,
			error: {}
		});
	});
}

module.exports = app;
