var express = require('express');
var router = express.Router();

var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/../../config/config.json')[env];

/* GET home page. */
router.get('/', function(req, res, next) {
  res.json({ title: 'Tab Functions' });
});

router.use('/status', require('./status'));

module.exports = router;

