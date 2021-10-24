// Wrap require with ESM to allow ES6 module import syntax
// eslint-disable-next-line no-global-assign
// module.exports = require('./app.js');
require = require('esm')(module);
module.exports = require('./app.js');
