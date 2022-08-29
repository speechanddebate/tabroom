const caseList = (sequelize, DataTypes) => {
	return sequelize.define('caseList', {
		slug: {
			type: DataTypes.STRING(255),
			allowNull: false,
			defaultValue: '',
		},
		eventcode: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	},{
		tableName : 'caselist',
	});

};

export default caseList;
