const rating = (sequelize, DataTypes) => {
	return sequelize.define('rating', {
		type : {
			type: DataTypes.ENUM('school', 'entry', 'coach'),
			allowNull: true,
		},
		draft : {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0,
		},
		entered : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		ordinal: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		percentile: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
	});
};

export default rating;
