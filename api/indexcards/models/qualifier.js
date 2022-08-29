const qualifier = (sequelize, DataTypes) => {
	return sequelize.define('qualifier', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
		},
		result: {
			type: DataTypes.STRING(127),
			allowNull: true,
		},
	});
};

export default qualifier;
