const Share = {
	type : 'object',
	properties : {
		panels      : { type : 'array', nullable  : true },
		files       : { type : 'array', nullable  : true },
		share_key   : { type : 'string' , nullable  : true },
		from        : { type : 'string' , nullable  : true },
	},
};

export default Share;
