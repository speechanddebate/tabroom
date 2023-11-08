// if (typeof window === 'undefined') {
//     require('dotenv').config(); // eslint-disable-line global-require
// }

const config = {
    API_URL: process.env.API_URL || 'http://localhost:10010/v1',
};

const env = process.env.NODE_ENV || 'development';

switch (env) {
    case 'production':
        config.API_URL = 'https://api.tabroom.com/v1';
        break;

    case 'test':
    case 'staging':
    case 'development':
    default:
        break;
}

// Object.keys(config).forEach(key => {
//     if (config[key] === 'true') { config[key] = true; }
//     if (config[key] === 'false') { config[key] = false; }
// });

export default config;
