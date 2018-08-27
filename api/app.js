var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');

var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/config/config.json')[env];

var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var crypt = require('crypt3');


var app = express();

	// View engine setup -- Jade
	app.set('views', path.join(__dirname, 'views'));
	app.set('view engine', 'jade');

	app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
	app.use(logger('dev'));
	app.use(bodyParser.json());
	app.use(bodyParser.urlencoded({ extended: false }));

	app.use(cookieParser());
	app.use(express.static(path.join(__dirname, 'public')));

	// Check for a valid session and populate to the req.session
	var authenticate = require('./lib/user/authenticate');

	app.use(function(req, res, next) { 
		authenticate(req,res,next);
	});

	// If there is an active login, load the permissions feed into

	// Set the URL routes
	app.use('/', require('./routes'));
	app.use('/user', require('./routes/user'));
	//app.use('/nsdatab', require('./routes/nsdatab'));

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
			res.render('error', {
				message: err.message,
				error: err
			});
		});
	}

	// Production error handler
	// No stacktraces leaked to user
	app.use(function(err, req, res, next) {
		res.status(err.status || 500);
		res.render('error', {
			message: err.message,
			error: {}
		});
	});


module.exports = app;
