// Wrap require with ESM to allow ES6 module import syntax
// eslint-disable-next-line no-global-assign
// module.exports = require('./app.js');
require = require('esm-wallaby')(module);
module.exports = require('./app.js');
