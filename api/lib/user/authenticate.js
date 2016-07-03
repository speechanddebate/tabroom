
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


	var authenticate = function (username, password) { 

		return new BluePromise ( function(resolve, reject) { 

			var localSession = {};

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
					reject(error);
				}

				if (Sessions) { 

					var Session = Sessions[0];
					var cryptString = Session.id.toString()+config.sessionSalt;
					var cryptHash = req.cookies[config.cookieName];

					if (Session) { 

						if (crypt(cryptString, cryptHash) === cryptHash) {

							localSession.id = Session.id;

							if (Session.su_id) { 

								localSession.user = Session.person.id;
								localSession.username = Session.person.email;
								localSession.realname = Session.person.first;
								if (Session.person.middle) { 
									localSession.realname += " "+Session.person.middle;
								}
								localSession.realname += " "+Session.person.last;
								localSession.site_admin = Session.person.site_admin;

							} else { 

								localSession.user = Session.person.id;
								localSession.username = Session.person.email;
								localSession.realname = Session.person.first;
								if (Session.person.middle) { 
									localSession.realname += " "+Session.person.middle;
								}
								localSession.realname += " "+Session.person.last;
								localSession.site_admin = Session.person.site_admin;
							}

							resolve(localSession);

						}
					}
				}

				reject("No valid session found");

			});

			reject("No session found");
			//END OF PROMISE
	
		});
	};


	module.exports = authenticate;
