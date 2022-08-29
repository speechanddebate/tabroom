const conflict = (sequelize, DataTypes) => {
	return sequelize.define('conflict', {
		type: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: '',
		},
	});
};

export default conflict;
