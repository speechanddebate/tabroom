const housing = (sequelize, DataTypes) => {
	return sequelize.define('housing', {
		type: {
			type: DataTypes.STRING(7),
			allowNull: true,
		},
		night: {
			type: DataTypes.DATEONLY,
			allowNull: false,
			defaultValue: '0000-00-00',
		},
		waitlist: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0,
		},
		tba: {
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: 0,
		},
		requested: {
			type: DataTypes.DATE,
			allowNull: false,
			defaultValue: '0000-00-00 00:00:00',
		},
	});
};

export default housing;
