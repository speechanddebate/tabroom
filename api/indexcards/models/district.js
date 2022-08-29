const district = (sequelize, DataTypes) => {
	return sequelize.define('district', {
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: '',
		},
		code: {
			type: DataTypes.STRING(16),
			allowNull: true,
		},
		location: {
			type: DataTypes.STRING(16),
			allowNull: true,
		},
		level: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		realm: {
			type: DataTypes.STRING(8),
			allowNull: true,
		},
	});
};

export default district;
