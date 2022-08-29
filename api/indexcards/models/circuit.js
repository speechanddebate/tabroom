const circuit = (sequelize, DataTypes) => {
	return sequelize.define('circuit', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		abbr: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		tz: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		active: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0,
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		country: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		webname: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
	});
};

export default circuit;
