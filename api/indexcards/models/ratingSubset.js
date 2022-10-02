const ratingSubset = (sequelize, DataTypes) => {
	return sequelize.define('ratingSubset', {
		name: {
			type: DataTypes.STRING(32),
			allowNull: true,
		},
	},{
		tableName: 'rating_subset',
	});
};

export default ratingSubset;
