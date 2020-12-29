require('dotenv').config();

// Default (Dev config)
const config = {
    PORT                  : 9876,
    RATE_WINDOW           : 15 * 60 * 1000,
    RATE_MAX              : 100000,
    RATE_DELAY            : 0,
    DB_HOST               : 'localhost',
    DB_PORT               : '3306',
    DB_USER               : 'username',
    DB_PASS               : 'password',
    DB_DATABASE           : 'tabroom',
    DB_CONNECTION_LIMIT   : 500,
    DB_CONNECTION_TIMEOUT : 60000,
    DB_RETRIES            : 5,
    DB_RETRY_DELAY        : 100,
    LOGIN_URL             : 'http://local.tabroom.com/user/login/login.mhtml',
    MAIL_FROM             : 'live@tabroom.com',
	MAIL_SERVER           : 'localhost',
	LOGIN_TOKEN			  : 'random-long-string',
	JITSI_KEY             : 'campus-jitsi-key',
	JITSI_URI             : 'https://campus.speechanddebate.org',
	NSDA_API_USER		  : 'nsda-api-userId',
	NSDA_API_KEY		  : 'nsda-api-password',
	NSDA_API_ENDPOINT     : 'https://api.speechanddebate.org',
	NSDA_API_PATH         : '/v2',
	LDAP_SERVER           : 'ldapserver.domain.com',
	LDAP_PORT		      : '636',
	LDAP_DN               : 'dc=tabroom,dc=com',
	LDAP_USER             : 'cn=admin,dc=tabroom,dc=com',
	LDAP_PW               : 'ldapAdminPasswordHere',
	S3_BUCKET			  : 's3://tabroom-files',
	S3_URL				  : 'https://s3.amazonaws.com/tabroom-files/tourns',
	NAUDL_USERNAME		  : 'somethingfrom@salesforce.com',
	NAUDL_PW			  : 'askLukeHill',
	NAUDL_TOURN_ENDPOINT  : 'tournamentServiceUrl',
	NAUDL_STUDENT_ENDPOINT: 'studentServiceTabroomUrl',
	NAUDL_STA_ENDPOINT    : 'staTabroomServiceUrl',
};

const env = process.env.NODE_ENV || 'development';

switch (env) {
    case 'test':
        break;

    case 'staging':
        config.DB_HOST = 'localhost';
        config.DB_USER = 'tabroom';
        config.DB_PASS = '';
        break;

    case 'production':
        config.DB_HOST    = 'db.speechanddebate.org';
        config.DB_USER    = 'tabroom';
        config.DB_PASS    = '';
        break;

    case 'development':
    default:
        break;
}

// Override any config value if corresponding env var is set
const configKeys = Object.keys(config);
const envKeys = Object.keys(process.env);

configKeys.forEach((key) => {
    if (envKeys.includes(key)) {
        config[key] = process.env[key];
    }
    if (config[key] === 'true') { config[key] = true; }
    if (config[key] === 'false') { config[key] = false; }
});


export default config;
