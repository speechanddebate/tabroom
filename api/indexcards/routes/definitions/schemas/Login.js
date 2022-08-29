import { mysqlDate } from '../formats';

const Login = {
	type : 'object',
	properties : {
		id                  : { type : 'integer' },
		username            : { type : 'string'  },
		sha512              : { type : 'string'  , nullable : true },
		accesses            : { type : 'integer' },
		last_access         : { type : 'string'  , nullable : true , pattern : mysqlDate },
		pass_timestamp      : { type : 'string'  , nullable : true , pattern : mysqlDate },
		pass_changekey      : { type : 'string'  , nullable : true },
		pass_change_expires : { type : 'string'  , nullable : true , pattern : mysqlDate },
		person              : { type : 'integer' },
		timestamp           : { type : 'string'  , nullable : true , pattern : mysqlDate },
	},
};

export default Login;
