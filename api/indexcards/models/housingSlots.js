const housingSlots = (sequelize, DataTypes) => {
	return sequelize.define('housingSlots', {
		night: {
			type: DataTypes.DATEONLY,
			allowNull: false,
			defaultValue: '0000-00-00',
		},
		slots: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	},{
		tableName: 'housing_slots',
	});
};

export default housingSlots;
