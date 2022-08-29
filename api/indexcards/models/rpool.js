const rpool = (sequelize, DataTypes) => {
	return sequelize.define('rpool', {
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
		},
	});
};

export default rpool;
