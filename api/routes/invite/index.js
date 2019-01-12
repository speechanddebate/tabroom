var express = require('express');
var router = express.Router();
var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/../../config/config.json')[env];

var db      = require('../../models');

/* GET home page. */
router.get('/', function(req, res, next) {
  res.json({title: 'Public Tournament and Results Data' });
});

router.get('/:tournID', function(req, res, next) {
	db.tourn.findById(req.params.tournID).then(function(Tourn) { 
		res.json(Tourn);
	});
});

router.get('/:tournID/pages', function(req, res, next) {
	db.webpage.findAll({ 
		where: { tourn: req.params.tournID, published : 1},
		order: [["page_order"]]
	}).then(function(Pages) { 
		res.json(Pages);
	});
});

router.get('/:tournID/files', function(req, res, next) {
	db.file.findAll({ 
		where: { tourn: req.params.tournID },
	}).then(function(Files) { 
		res.json(Files);
	});
});

router.get('/:tournID/events', function(req, res, next) {
	db.event.findAll({ 
		where: { tourn: req.params.tournID },
		order: [["abbr"]],
		include: [{ 
			model : db.event_setting,
			as    : "EventSettings"
		}]
	}).then(function(Events) { 
		res.json(Events);
	});
});

router.get('/:tournID/rounds/published', function(req, res, next) {
	db.sequelize.query(`select round.*, 
		event.abbr as eventAbbr, event.name as eventName, 
		timeslot.start as timeslotStart, timeslot.end as timeslotEnd
			from (round, event, timeslot)
			where event.tourn = ? 
			and event.id = round.event
			and round.timeslot = timeslot.id
			and round.published > 0`,
		{raw: true, replacements: [req.params.tournID] }
	).then(function (Rounds) { 
		res.json(Rounds);
	});
});

router.get('/:tournID/rounds/results', function(req, res, next) {
	db.sequelize.query(`select round.*, 
		event.abbr as eventAbbr, event.name as eventName, 
		timeslot.start as timeslotStart, timeslot.end as timeslotEnd
			from (round, event, timeslot)
			where event.tourn = ? 
			and event.id = round.event
			and round.timeslot = timeslot.id
			and round.post_results > 0`,
		{raw: true, replacements: [req.params.tournID] }
	).then(function (Rounds) { 
		res.json(Rounds);
	});
});

router.get('/name/:webName', function(req, res, next) {

	const webName = req.params.webName;

	if (webName === parseInt(webName)) { 
		db.tourn.findById(req.params.webName).then(function(Tourn) { 
			res.json(Tourn);
		});
	} else { 
		db.tourn.findOne({
			where : {webname: webName},
			order : [[ 'start', 'DESC' ]]
		}).then(function(Tourn) { 
			res.json(Tourn);
		});
	}
});

router.use('/tourn', require('./tourn'));

module.exports = router;

