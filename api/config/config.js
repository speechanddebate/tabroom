require('dotenv').config();

// Default (Dev config)
const config = {
    PORT                  : 10010,
    RATE_WINDOW           : 15 * 60 * 1000,
    RATE_MAX              : 100000,
    RATE_DELAY            : 0,
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
	COOKIE_NAME           : 'TabroomToken',

    DB_HOST               : 'localhost',
    DB_PORT               : '3306',
    DB_USER               : 'username',
    DB_PASS               : 'password',
    DB_DATABASE           : 'tabroom',
	sequelizeOptions      : {
		"dialect"         : "mariadb",
		"freezeTableName" : true,
		"underscored"     : true,
		"timestamps"      : false
	}
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
		config.port       = 3000,
        config.DB_HOST    = 'db.speechanddebate.org';
        config.DB_USER    = 'tabroom';
        config.DB_PASS    = '';
		config.sequelizeOptions.pool = {
			"max"     : 50,
			"min"     : 5,
			"acquire" : 30000,
			"idle"    : 10000
		};
        break;

    case 'development':
    default:
        break;
}

config.sequelizeOptions.host = config.DB_HOST;

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
