var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');

var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/config/config.json')[env];

var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var crypt = require('crypt3');

var db = require(__dirname + '/models');


var app = express();

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

// uncomment after placing your favicon in /public
app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.get('/', function(req, res, next) { 

	db.session.findAll(
		{ 
			where: { userkey: req.cookies[config.cookieName] },
			include: [
				{ model: db.person, required: true },
				{ model: db.person, as: "su", required: false}
			]
		}
	).then(function(Sessions, error) { 

		if (error) { 
			console.log("Error in session lookup: "+error);
			res.json("Error in session lookup found: "+error);
		}

		if (Sessions) { 

			var Session = Sessions[0];
			console.log(Session);

			var cryptString = Session.id.toString()+config.sessionSalt;
			var cryptHash = req.cookies[config.cookieName];

			if (Session) { 

				if (crypt(cryptString, cryptHash) === cryptHash) {

					res.locals.session = Session;

					if (Session.su_id) { 

						res.locals.user = Session.person.id;
						res.locals.username = Session.person.email;
						res.locals.realname = Session.person.first;
						if (Session.person.middle) { 
							res.locals.realname += " "+Session.person.middle;
						}
						res.locals.realname += " "+Session.person.last;
						res.locals.site_admin = Session.person.site_admin;

					} else { 

						res.locals.user = Session.person.id;
						res.locals.username = Session.person.email;
						res.locals.realname = Session.person.first;
						if (Session.person.middle) { 
							res.locals.realname += " "+Session.person.middle;
						}
						res.locals.realname += " "+Session.person.last;
						res.locals.site_admin = Session.person.site_admin;
					}

				}
			}
		}

		next();

	});

});

app.use('/', require('./routes'));
app.use('/user', require('./routes/user'));

// catch 404 and forward to error handler
app.use(function(req, res, next) {
  var err = new Error('Not Found');
  err.status = 404;
  next(err);
});

// error handlers

// development error handler
// will print stacktrace
if (app.get('env') === 'development') {
  app.use(function(err, req, res, next) {

    res.status(err.status || 500);
    res.render('error', {
      message: err.message,
      error: err
    });
  });
}

// production error handler
// no stacktraces leaked to user
app.use(function(err, req, res, next) {
  res.status(err.status || 500);
  res.render('error', {
    message: err.message,
    error: {}
  });
});


module.exports = app;
