const resultValue = (sequelize, DataTypes) => {
	return sequelize.define('resultValue', {
		table_name: 'result_value',
		value: {
			type: DataTypes.TEXT('medium'),
			allowNull: true,
		},
		priority: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	});
};

export default resultValue;
