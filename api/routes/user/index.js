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

router.get('/:userid', function(req, res, next) { 

	if (req.session.user === req.params.userid || req.session.site_admin) { 
		db.person.findById(req.params.userid).then( function(Person) { 
			res.json(Person);
		});
	} else { 
		res.json({error: "Not Authorized"});
	}
});

router.get('/perms/:userid', function(req, res, next) { 

	if (req.session.user === req.params.userid || req.session.site_admin) { 
		Permissions(req.params.userid, router.locals).then(function(Perms) {
			res.json(Perms);
		});
	} else { 
		res.json({error: "Not Authorized"});
	}

});

module.exports = router;


