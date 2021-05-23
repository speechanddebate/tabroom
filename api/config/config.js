require('dotenv').config();

// Default (Dev config)
const config = {
    PORT                  : 10010,
    RATE_WINDOW           : 15 * 60 * 1000,
    RATE_MAX              : 100000,
    RATE_DELAY            : 0,
    LOGIN_URL             : 'http://www.tabroom.com/user/login/login.mhtml',
    MAIL_FROM             : 'live@tabroom.com',
	MAIL_SERVER           : 'localhost',
	LOGIN_TOKEN			  : 'I/L6[K8tA*33G>7',
	JITSI_KEY             : 'g2349df9023',
	JITSI_URI             : 'https://campus.speechanddebate.org',
	NSDA_API_USER		  : '10382636',
	NSDA_API_KEY		  : 'AG4DHCeblnf3TAp7',
	NSDA_API_ENDPOINT     : 'https://api.speechanddebate.org',
	NSDA_API_PATH         : '/v2',
	LDAP_SERVER           : 'baldur.tabroom.com',
	LDAP_PORT		      : '389',
	LDAP_DN               : 'dc=tabroom,dc=com',
	LDAP_USER             : 'cn=admin,dc=tabroom,dc=com',
	LDAP_PW               : 'trMkodb8>W,+t*h8T',
	S3_BUCKET			  : 's3://tabroom-files',
	S3_URL				  : 'https://s3.amazonaws.com/tabroom-files/tourns',
	NAUDL_USERNAME		  : 'salesforce@tabroom.com',
	NAUDL_PW			  : 'Ax5GDwi#NEgW#RrTnGvzvJciD6WuTeE',
	NAUDL_TOKEN			  : 'JexH9GRxXycAvkxaf4f0f9Obr',
	NAUDL_HOST			  : 'https://cs45.salesforce.com',
	NAUDL_TOURN_ENDPOINT  : '/services/apexrest/v.1/TournamentService',
	NAUDL_STUDENT_ENDPOINT: '/services/apexrest/v.1/StudentServiceTabroom',
	NAUDL_STA_ENDPOINT    : '/services/apexrest/v.1/STATabroomService',
	COOKIE_NAME           : 'TabroomToken',
 	DB_DATABASE           : 'tabroom',
	sequelizeOptions      : {
		"dialect"         : "mariadb",
		"define" : {
			"freezeTableName" : true,
			"modelName"       : "singularName",
			"underscored"     : true,
			"timestamps"      : false
		}
	}
};

const env = process.env.NODE_ENV || 'development';

switch (env) {
    case 'test':
	config.sequelizeOptions.logging = false;
        config.DB_HOST = 'localhost';
        config.DB_USER = 'tabroom';
        config.DB_PASS = 'C3Eil-aiQuaiseigoo4hee2YooG';
        break;

    case 'staging':
        config.DB_HOST = 'localhost';
        config.DB_USER = 'tabroom';
        config.DB_PASS = 'C3Eil-aiQuaiseigoo4hee2YooG';
        break;

    case 'stagetest':
	config.PORT       = 3000,
        config.DB_HOST    = 'db.speechanddebate.org';
        config.DB_USER    = 'tabroom';
        config.DB_PASS    = 'wahf3eicaic[ooxi7YahXahJ';
	config.sequelizeOptions.pool = {
		"max"     : 1,
		"min"     : 1,
		"acquire" : 30000,
		"idle"    : 10000
	};
	config.sequelizeOptions.logging = false;
        break;

    case 'production':
	config.PORT       = 3000,
        config.DB_HOST    = 'db.speechanddebate.org';
        config.DB_USER    = 'tabroom';
        config.DB_PASS    = 'wahf3eicaic[ooxi7YahXahJ';
	config.sequelizeOptions.pool = {
		"max"     : 50,
		"min"     : 5,
		"acquire" : 30000,
		"idle"    : 10000
	};
	config.sequelizeOptions.logging = false;
        break;

    case 'development':
        config.DB_HOST = 'localhost';
        config.DB_USER = 'tabroom';
        config.DB_PASS = 'C3Eil-aiQuaiseigoo4hee2YooG';
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
