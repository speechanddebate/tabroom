// This library will create a function whereby a user ID is returned with a
// tree of active permissions for the user.  Future tournaments, judges, and
// current student records and school records are all covered and returned as a
// json object to the caller.

// Because I'm learning node LIKE A BOSS, I'm going to return a promise instead
// of dealing with callback hell.  Or die trying.  

var env    = process.env.NODE_ENV || "development";
var config = require('../../config/config.json')[env];

var db = require('../../models');

var crypt = require('crypt3');
var BluePromise = require('bluebird');

	var authenticate = function(req, res, next) { 

		db.session.findAll(
			{ 
				where: { userkey: req.cookies[config.cookieName] },
				include: [
					{ model: db.person, required: true },
					{ model: db.person, as: "su", required: false}
				]
			}

		).then(function(Sessions) { 

			if (Sessions) { 

				var Session = Sessions[0];
				var cryptString = Session.id.toString()+config.sessionSalt;
				var cryptHash = req.cookies[config.cookieName];

				if (Session) { 

					if (crypt(cryptString, cryptHash) === cryptHash) {

						res.locals.session = Session.id;

						if (Session.su) { 
							res.locals.su = Session.su.id;
							req.app.su = Session.su.id;
						}

						res.locals.user = Session.person.id;
						res.locals.username = Session.person.email;
						res.locals.realname = Session.person.first;
						if (Session.person.middle) { 
							res.locals.realname += " "+Session.person.middle;
						}
						res.locals.realname += " "+Session.person.last;
						res.locals.site_admin = Session.person.site_admin;

						req.session = { 
							user       : res.locals.user,
							site_admin : res.locals.site_admin,
							event      : Session.event_id,
							category   : Session.category_id,
						};

					}
				}
			}
						
			next();

		});

	};

	module.exports = authenticate;

