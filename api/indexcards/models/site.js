const site = (sequelize, DataTypes) => {
	return sequelize.define('site', {
		name: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: '',
		},
		directions: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		dropoff: {
			type: DataTypes.STRING,
			allowNull: true,
		},
		online: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: false,
		},
	});
};

export default site;
