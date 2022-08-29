const result = (sequelize, DataTypes) => {
	return sequelize.define('result', {
		rank: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		place: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		percentile: {
			type: DataTypes.DECIMAL(6,2),
			allowNull: true,
		},
	});
};

export default result;
