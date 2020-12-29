// Wrap require with ESM to allow ES6 module import syntax
// eslint-disable-next-line no-global-assign
require = require('esm')(module);
module.exports = require('./app.js');
