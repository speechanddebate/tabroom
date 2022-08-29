const jpool = (sequelize, DataTypes) => {
	return sequelize.define('jpool', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
		},
	});
};

export default jpool;
