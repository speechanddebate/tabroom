var express = require('express');
var router = express.Router();

var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/../../config/config.json')[env];
var db = require(__dirname + '/../../models');
var Permissions = require('../../lib/user/permissions');

router.get('/', function(req, res, next) {
  res.render('index', { title: 'Status Functions' });
});

router.get('/:tournId', function(req, res, next) { 


	if (req.session.user) { 

		Permissions(req.session.user, router.locals, req.params.tournId).then(function(Perms) {

			if (Perms || req.session.site_admin) { 

				res.json({result: "Authorized!"});
			
			} else { 
		
				res.json({error: "Not Authorized"});

			}
		});

	} else { 
		res.json({error: "Not Authorized"});
	}

});

module.exports = router;


