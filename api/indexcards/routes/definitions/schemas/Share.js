const Share = {
	type : 'object',
	properties : {
		panel       : { type : 'string', nullable  : true },
		round_id    : { type : 'integer', nullable  : true },
		file        : { type : 'string' , nullable  : true },
		share_key   : { type : 'string' , nullable  : true },
	},
};

export default Share;
