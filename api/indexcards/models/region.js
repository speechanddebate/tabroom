const region = (sequelize, DataTypes) => {
	return sequelize.define('region', {
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: '',
		},
		code: {
			type: DataTypes.STRING(8),
			allowNull: true,
		},
	});
};

export default region;
