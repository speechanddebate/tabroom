import { mysqlDate } from '../formats';

const Chapter = {
	type : 'object',
	properties : {
		id         : { type : 'integer' },
		name       : { type : 'string'  , nullable  : true },
		street     : { type : 'string'  , nullable  : true },
		city       : { type : 'string'  , nullable  : true },
		state      : { type : 'string'  , nullable  : true },
		zip        : { type : 'integer' , nullable  : true },
		postal     : { type : 'string'  , nullable  : true },
		country    : { type : 'string'  , nullable  : true },
		coaches    : { type : 'string'  , nullable  : true },
		self_prefs : { type : 'boolean' },
		level      : { type : 'string'  , nullable  : true },
		nsda       : { type : 'integer' , nullable  : true },
		district   : { type : 'integer' , nullable  : true },
		naudl      : { type : 'boolean' },
		ipeds      : { type : 'string'  , nullable  : true },
		nces       : { type : 'string'  , nullable  : true },
		ceeb       : { type : 'string'  , nullable  : true },
		timestamp  : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default Chapter;
