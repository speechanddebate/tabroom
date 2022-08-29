import { mysqlDate } from '../formats';

const Round = {
	type : 'object',
	properties : {
		id         : { type : 'integer' },
		name       : { type : 'string'  , nullable  : true },
		timestamp  : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default Round;
