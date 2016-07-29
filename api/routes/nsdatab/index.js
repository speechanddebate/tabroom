var express = require('express');
var router = express.Router();

var env    = process.env.NODE_ENV || "development";
/* var config = require(__dirname + '/../config/config.json')[env]; */
/* var db = require(__dirname + '/../models'); */
var db = require('../../models');

/* GET main nsda judge page. */
//router.get('/', function(req, res, next) {
//  res.render('nsda_judges', { title: 'Gonna sort the crap out of the judges' });
//});

// this totally works!
//router.get('/p/:id', function(req, res) {
//	var judgeID = req.params.id;
//	console.log("this has read judge id as "+req.params.id);
//	db.judge.find({where: {id: judgeID}}).then(function(Judge) { 
//		res.json(Judge);
//	});
//});

//works for this URL: http://localhost:3000/nsdatab/p/10699
router.get('/p/:id', function(req, res) {
	console.log("this has read judge category as "+req.params.id);
	var web_category_id = req.params.id;
	db.judge.findAll({ where: {category_id: web_category_id}}).then(function(Judges) {
		//res.json(Judges);
		var rds_commit = 0;
		for (var index in Judges) {
		    //	console.log("Judge last name is "+Judges[index].last);
		    rds_commit += Judges[index].obligation;
		}
		res.render('nsda_judges', { title: ' Judge Category List', result: Judges, rds_commit: rds_commit });

	});
});

router.post('/adduser', function(req, res) {
    console.log("You have totally clicked the button");
    next();
});

module.exports = router;


