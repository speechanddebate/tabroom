const changeLog = (sequelize, DataTypes) => {
	return sequelize.define('changeLog', {
		tag: {
			type: DataTypes.STRING(63),
			allowNull: false,
		},
		description: {
			type: DataTypes.TEXT('medium'),
			allowNull: false,
		},
		count: {
			type      : DataTypes.INTEGER,
			allowNull : true,
		},
		deleted: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false,
		},
		createdAt: {
			type      : DataTypes.DATE,
			allowNull : true,
			field     : 'created_at',
			defaultValue: new Date(),
		},
	}, {
		tableName : 'change_log',
	});
};

export default changeLog;
