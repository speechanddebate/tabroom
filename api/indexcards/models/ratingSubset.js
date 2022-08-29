const ratingSubset = (sequelize, DataTypes) => {
	return sequelize.define('ratingSubset', {
		table_name: 'rating_subset',
		name: {
			type: DataTypes.STRING(32),
			allowNull: true,
		},
	});
};

export default ratingSubset;
