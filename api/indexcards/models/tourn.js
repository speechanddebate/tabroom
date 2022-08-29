const tourn = (sequelize, DataTypes) => {
	return sequelize.define('tourn', {
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: '',
		},
		city: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		country: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		tz: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		webname: {
			type: DataTypes.STRING(64),
			allowNull: true,
		},
		hidden: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		reg_start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		reg_end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default tourn;
