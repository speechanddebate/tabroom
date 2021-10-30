import { mysqlDate } from '../formats.js';

const SchoolSetting = {
	type           : 'object',
	properties     : {
		id         : { type    : 'integer' },
		school     : { type    : 'integer' },
		tag        : { type    : 'string'  , nullable  : true },
		value      : { type    : 'string'  , nullable  : true },
		value_date : { type    : 'string'  , nullable  : true, pattern  : mysqlDate },
		value_text : { type    : 'boolean' },
		timestamp  : { type    : 'string'  , nullable  : true , pattern : mysqlDate },
	},
};

export default SchoolSetting;
