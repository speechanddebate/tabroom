const concessionType = (sequelize, DataTypes) => {
	return sequelize.define('concessionType', {
		name: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		description : {
			type: DataTypes.TEXT,
			allowNull: true,
		},
	},{
		tableName: 'concession_type',
	});
};

export default concessionType;
