import { mysqlDate } from '../formats.js';

const School = {
	type : 'object',
	properties : {
		id        : { type : 'integer' },
		name      : { type : 'string'  , nullable  : true },
		code      : { type : 'string'  , nullable  : true },
		state     : { type : 'string'  , nullable  : true },
		onsite    : { type : 'boolean' },
		tourn     : { type : 'integer'  , nullable : true },
		chapter   : { type : 'integer'  , nullable : true },
		region    : { type : 'integer'  , nullable : true },
		district  : { type : 'integer'  , nullable : true },
		timestamp : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default School;
