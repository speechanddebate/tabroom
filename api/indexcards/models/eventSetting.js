const eventSetting = (sequelize, DataTypes) => {
	return sequelize.define('eventSetting', {
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
		tableName : 'event_setting',
	});
};

export default eventSetting;
