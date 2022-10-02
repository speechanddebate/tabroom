const chapterSetting = (sequelize, DataTypes) => {
	return sequelize.define('chapterSetting', {
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
		tableName : 'chapter_setting',
	});
};

export default chapterSetting;
