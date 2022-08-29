const chapter = (sequelize, DataTypes) => {
	return sequelize.define('chapter', {
		name: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: '',
		},
		street: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		city: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		zip : {
			type: DataTypes.INTEGER(9),
			allowNull: true,
		},
		postal : {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		country: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		level: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
		},
		naudl: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
	});
};

export default chapter;
