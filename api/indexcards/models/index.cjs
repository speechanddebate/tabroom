require('fs').readdirSync(`${__dirname}/`).forEach(file => {
	if (file.match(/\.cjs$/) !== null && file !== 'index.cjs') {
		const name = file.replace('.cjs', '');
		module.exports[name] = require(`./${file}`);
	}
});
