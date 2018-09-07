var express = require('express');
var router  = express.Router();
var db      = require('../../models');

/* GET users listing. */

router.get('/', function(req, res, next) {
	res.json({message: "List future tournaments"});
});

router.get('/:tournId', function(req, res, next) { 
	res.json({message: "Details of a tournament"});
});

module.exports = router;


