const shift = (sequelize, DataTypes) => {
	return sequelize.define('shift', {
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: '',
		},
		type: {
			type: DataTypes.ENUM('signup', 'strike', 'both'),
			allowNull: false,
			defaultValue: 'string',
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		fine: {
			type: DataTypes.FLOAT,
			allowNull: true,
		},
		no_hires: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: false,
		},
	});
};

export default shift;
