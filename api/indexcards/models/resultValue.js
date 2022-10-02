const resultValue = (sequelize, DataTypes) => {
	return sequelize.define('resultValue', {
		value: {
			type: DataTypes.TEXT('medium'),
			allowNull: true,
		},
		priority: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	},{
		tableName: 'result_value',
	});
};

export default resultValue;
