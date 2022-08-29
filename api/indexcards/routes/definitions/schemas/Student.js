import { mysqlDate } from '../formats';

const Student = {
	type          : 'object',
	properties    : {
		id       : { type : 'integer' },
		first    : { type : 'string'  , nullable : true },
		middle   : { type : 'string'  , nullable : true },
		last     : { type : 'string'  , nullable : true },
		phonetic : { type : 'string'  , nullable : true },
		grad_year : { type : 'integer'  , nullable : true },
		novice   : { type : 'boolean' },
		retired   : { type : 'boolean' },
		gender : { type : 'string'  , nullable : true },
		diet : { type : 'string'  , nullable : true },
		birthdate  : { type : 'string'  , nullable  : true , pattern : mysqlDate },
		school_sid : { type : 'string'  , nullable : true },
		race : { type : 'string'  , nullable : true },
		nsda       : { type : 'integer', nullable: true },
		chapter       : { type : 'integer', nullable: true },
		person       : { type : 'integer', nullable: true },
		person_request       : { type : 'integer', nullable: true },
		timestamp  : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default Student;
