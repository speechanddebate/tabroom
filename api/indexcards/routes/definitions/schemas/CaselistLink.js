const CaselistLink = {
	type : 'object',
	properties : {
		person_id    : { type : 'integer' },
		slug         : { type : 'string'  , nullable  : true },
		caselist_key : { type : 'string'  , nullable  : true },
		eventcode    : { type : 'integer',  nullable  : true },
	},
};

export default CaselistLink;
