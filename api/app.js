const express = require('express');
const path = require('path');

const env    = process.env.NODE_ENV || "development";
const config = require(__dirname + '/config/config.json')[env];

const logger = require('morgan');
const cookieParser = require('cookie-parser');
const bodyParser = require('body-parser');
const crypt = require('crypt3');

var app = express();
app.use(logger('dev'));

// BodyParser allows one to access the data from POSTs
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));

// CookieParser reads in the data from cookies for auth
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

// Development error handler
// Prints stacktrace
if (app.get('env') === 'development') {
	app.use(function(err, req, res, next) {
		res.status(err.status || 500);
		res.json({
			message: err.message,
			error: err
		});
	});
}

// Production error handler
// No stacktraces leaked to user
app.use(function(err, req, res, next) {
	res.status(err.status || 500);
	res.json({
		message: err.message,
		error: {}
	});
});

module.exports = app;
