const circuitSetting = (sequelize, DataTypes) => {
	return sequelize.define('circuitSetting', {
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
		},
		value: {
			type: DataTypes.STRING(64),
			allowNull: true,
		},
		value_text: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		value_date: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	},{
		tableName : 'circuit_setting',
	});
};

export default circuitSetting;
