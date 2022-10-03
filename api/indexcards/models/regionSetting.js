const regionSetting = (sequelize, DataTypes) => {
	return sequelize.define('regionSetting', {
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
		tableName: 'region_setting',
	});
};

export default regionSetting;
