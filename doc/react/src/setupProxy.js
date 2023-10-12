// This file cannot be rewritten in ESM
const { createProxyMiddleware } = require('http-proxy-middleware');

module.exports = function(app) {
	console.log(process.env.REACT_APP_PROXY_HOST);
	app.use(
		'/v1',
		createProxyMiddleware({
			target: process.env.REACT_APP_PROXY_HOST,
			changeOrigin: true,
		})
	);
};
