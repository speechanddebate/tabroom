const hotel = (sequelize, DataTypes) => {
	return sequelize.define('hotel', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		multiple: {
			type: DataTypes.FLOAT,
			allowNull: true,
		},
		surcharge: {
			type: DataTypes.FLOAT,
			allowNull: true,
		},
	});
};

export default hotel;
