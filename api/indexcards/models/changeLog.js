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
		deleted          : {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false,
		},
		createdAt : {
			type      : DataTypes.DATE,
			allowNull : true,
			field     : 'created_at',
		},
	}, {
		tableName : 'change_log',
	});
};

export default changeLog;
