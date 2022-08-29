const concessionType = (sequelize, DataTypes) => {
	return sequelize.define('concessionType', {
		table_name: 'concession_type',
		name: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		description : {
			type: DataTypes.TEXT,
			allowNull: true,
		},
	});
};

export default concessionType;
