const Ad = {
	type : 'object',
	properties : {
		id             : { type : 'number'  , nullable  : true },
		filename       : { type : 'string'  , nullable  : true },
		url            : { type : 'string'  , nullable  : true },
	},
};

export default Ad;
