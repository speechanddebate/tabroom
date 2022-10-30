const protocol = (sequelize, DataTypes) => {
	return sequelize.define('protocol', {
		name: {
			type: DataTypes.STRING,
			allowNull: true,
		},
	},{
		tableName: 'protocol',
	});
};

export default protocol;
