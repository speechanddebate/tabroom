const tournSetting = (sequelize, DataTypes) => {
	return sequelize.define('tournSetting', {
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: '',
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
		tableName: 'tourn_setting',
	});
};

export default tournSetting;
