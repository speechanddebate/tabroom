const permission = (sequelize, DataTypes) => {
	return sequelize.define('permission', {
		id: {
			type: DataTypes.INTEGER,
			primaryKey: true,
			autoIncrement: true,
			allowNull: false,
		},
		tag: {
			type: DataTypes.STRING(16),
			allowNull: true,
		},
		details: {
			type: DataTypes.JSON,
			allowNull: true,
		},
	});
};

export default permission;
