const pattern = (sequelize, DataTypes) => {
	return sequelize.define('pattern', {
		name: {
			type: DataTypes.STRING(31),
			allowNull: false,
			defaultValue: '',
		},
		type: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		max: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
	});
};

export default pattern;
