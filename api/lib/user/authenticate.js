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
						
		req.session = { };

		if (req.cookies[config.cookieName]) { 

			db.session.findAll(
				{ 
					where: { userkey: req.cookies[config.cookieName] },
				}

			).then(function(Sessions) { 

				if (Sessions[0]) { 

					var Session = Sessions[0];

					var cryptString = Session.id.toString()+config.sessionSalt;
					var cryptHash = req.cookies[config.cookieName];

					if (Session) { 

						if (crypt(cryptString, cryptHash) === cryptHash) {

							res.locals.session = Session.id;

							if (Session.person) { 

								db.person.findById(Session.person).then(function(Person) { 

									res.locals.person = Person.id;
									res.locals.username = Person.email;
									res.locals.realname = Person.first;

									if (Person.middle) { 
										res.locals.realname += " "+Person.middle;
									}
									res.locals.realname += " "+Person.last;
									res.locals.site_admin = Person.site_admin;

									if (Session.su) { 
										res.locals.su = Session.su;
										req.app.su = Session.su;
									}

									req.session = { 
										person     : res.locals.person,
										site_admin : res.locals.site_admin,
									};
								
									next();

								});

							} else { 
							
								next();

							}
						}
					}
				}

			});

		} else { 

			next();

		}

	};

	module.exports = authenticate;

