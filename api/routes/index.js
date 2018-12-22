var express = require('express');
var router = express.Router();

var env    = process.env.NODE_ENV || "development";
var config = require(__dirname + '/../config/config.json')[env];

/* GET home page. */
router.get('/', function(req, res, next) {
  res.json({ greeting: 'Welcome to the Tabroom.com API' });
});

router.use('/invite', require('./invite'));
router.use('/tourn', require('./tourn'));
router.use('/user', require('./user'));
router.use('/admin', require('./admin'));
router.use('/school', require('./school'));

module.exports = router;

