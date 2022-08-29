const chapterSetting = (sequelize, DataTypes) => {
	return sequelize.define('chapterSetting', {
		table_name : 'chapter_setting',
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
	});
};

export default chapterSetting;
