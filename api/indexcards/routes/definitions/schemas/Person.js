import { mysqlDate } from '../formats';

const Person = {
	type          : 'object',
	properties    : {
		id       : { type : 'integer' },
		email    : { type : 'string'  , nullable : true },
		first    : { type : 'string'  , nullable : true },
		middle   : { type : 'string'  , nullable : true },
		last     : { type : 'string'  , nullable : true },
		street   : { type : 'string'  , nullable : true },
		city     : { type : 'string'  , nullable : true },
		state    : { type : 'string'  , nullable : true },
		zip      : { type : 'string'  , nullable : true },
		postal   : { type : 'string'  , nullable : true },
		country  : { type : 'string'  , nullable : true },
		tz       : { type : 'string'  , nullable : true },
		nsda     : { type : 'integer' , nullable : true },
		phone    : { type : 'integer' , nullable : true },
		provider : { type : 'string'  , nullable : true },
		gender   : { type : 'string'  , nullable : true },
		pronoun  : { type : 'string'  , nullable : true },
		no_email   : { type : 'boolean' },
		site_admin : { type : 'boolean' , nullable : true },
		timestamp  : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default Person;
