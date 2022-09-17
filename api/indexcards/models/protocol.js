const protocol = (sequelize, DataTypes) => {
	return sequelize.define('protocol', {
		name: {
			type: DataTypes.STRING,
			allowNull: true,
		},
	},{
		tableName: 'tiebreak_set',
	});
};

export default protocol;
