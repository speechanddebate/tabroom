const session = (sequelize, DataTypes) => {
	return sequelize.define('session', {
		userkey: {
			type: DataTypes.STRING(127),
			allowNull: false,
		},
		ip: {
			type: DataTypes.STRING(63),
			allowNull: false,
		},
		defaults: {
			type: DataTypes.JSON,
			allowNull: true,
		},
	});
};

export default session;
