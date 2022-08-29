const autoqueue = (sequelize, DataTypes) => {
	return sequelize.define('autoqueue', {
		tag: {
			type         : DataTypes.STRING(31),
			allowNull    : false,
			defaultValue : '',
		},
		message      : {
			type      : DataTypes.STRING,
			allowNull : true,
		},
		active_at         : {
			type      : DataTypes.DATE,
			allowNull : true,
		},
		created_at         : {
			type      : DataTypes.DATE,
			allowNull : true,
		},
	});
};

export default autoqueue;
