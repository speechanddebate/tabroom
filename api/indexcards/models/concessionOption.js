const concessionOption = (sequelize, DataTypes) => {
	return sequelize.define('concessionOption', {
		name: {
			type: DataTypes.STRING(8),
			allowNull: true,
		},
		description : {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		disabled : {
			type: DataTypes.BOOLEAN,
			allowNull: true,
		},
	},{
		tableName : 'concession_option',
	});
};

export default concessionOption;
