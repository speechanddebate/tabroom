const resultKey = (sequelize, DataTypes) => {
	return sequelize.define('resultKey', {
		tag: {
			type      : DataTypes.STRING(63),
			allowNull : true,
		},
		description: {
			type         : DataTypes.STRING(255),
			allowNull    : true,
		},
		no_sort: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		sort_desc: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
	},{
		tableName : 'result_key',
	});
};

export default resultKey;
