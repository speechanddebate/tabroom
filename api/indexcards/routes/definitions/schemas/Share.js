const Share = {
	type : 'object',
	properties : {
		panels      : { type : 'array', nullable  : true },
		file        : { type : 'string' , nullable  : true },
		filename    : { type : 'string' , nullable  : true },
		share_key   : { type : 'string' , nullable  : true },
	},
};

export default Share;
