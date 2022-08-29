const follower = (sequelize, DataTypes) => {
	return sequelize.define('follower', {
		type: {
			type: DataTypes.STRING(8),
			allowNull: false,
		},
		cell: {
			type: DataTypes.INTEGER(20),
			allowNull: false,
		},
		domain: {
			type: DataTypes.STRING(64),
			allowNull: true,
		},
		email: {
			type: DataTypes.STRING(64),
			allowNull: true,
		},
	});
};

export default follower;
