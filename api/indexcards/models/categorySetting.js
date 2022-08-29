const categorySetting = (sequelize, DataTypes) => {
	return sequelize.define('categorySetting', {
		tableName: 'category_setting',
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
		},
		value: {
			type: DataTypes.STRING(128),
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

export default categorySetting;
