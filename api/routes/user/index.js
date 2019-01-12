var express = require('express');
var router  = express.Router();
var db      = require('../../models');
var Permissions = require('../../lib/user/permissions');

/* GET users listing. */

router.get('/', function(req, res, next) {
	if (req.session.person) { 
		db.person.findById(req.session.person).then( function(Person) { 
			res.json(Person);
		});
	} else { 
		res.json({error: "No User Logged In"});
	}
});

router.get('/:userID', function(req, res, next) { 
	if (req.session.user === req.params.userID || req.session.site_admin) { 
		db.person.findById(req.params.userID).then( function(Person) { 
			res.json(Person);
		});
	} else { 
		res.json({error: "Not Authorized"});
	}
});

router.get('/:userID/perms', function(req, res, next) { 
	if (req.session.user === req.params.userID || req.session.site_admin) { 
		Permissions(req.params.userID, router.locals).then(function(Perms) {
			res.json(Perms);
		});
	} else { 
		res.json({error: "Not Authorized"});
	}
});

router.get('/:userID/perms/tourn/:tournID', function(req, res, next) { 

	if (req.session.user === req.params.userID || req.session.site_admin) { 
		Permissions(req.params.userID, router.locals, req.params.tournID).then(function(Perms) {
			res.json(Perms);
		});
	} else { 
		res.json({error: "Not Authorized"});
	}
});

module.exports = router;

