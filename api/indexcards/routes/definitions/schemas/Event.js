import { mysqlDate } from '../formats.js';

const Event = {
	type : 'object',
	properties : {
		id       : { type : 'integer' },
		name     : { type : 'string'  , nullable  : true },
		abbr     : { type : 'string'  , nullable  : true },
		type     : { type : 'string'  ,
			nullable  : false,
			enum : ['speech','congress','debate','wudc','wsdc'],
		},
		level    : { type : 'string'  ,
			nullable  : false,
			enum : ['open','jv','novice','champ','es-open','es-novice','middle'],
		},
		fee      : { type : 'number', format: 'float', nullable: true },
		tourn         : { type : 'integer'  , nullable : true },
		category      : { type : 'integer'  , nullable : true },
		pattern       : { type : 'integer'  , nullable : true },
		rating_subset : { type : 'integer'  , nullable : true },
		timestamp     : { type : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default Event;
