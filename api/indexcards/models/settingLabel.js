const settingLabel = (sequelize, DataTypes) => {
	return sequelize.define('settingLabel', {
		lang: {
			type: DataTypes.CHAR(2),
			allowNull: false,
		},
		label: {
			type: DataTypes.STRING(127),
			allowNull: false,
		},
		guide: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		options: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
	},{
		tableName: 'setting_label',
	});
};

export default settingLabel;
