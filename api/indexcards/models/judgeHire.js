const judgeHire = (sequelize, DataTypes) => {
	return sequelize.define('judgeHire', {
		entries_requested: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		entries_accepted: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		rounds_requested: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		rounds_accepted: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		requested_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	},{
		tableName: 'judge_hire',
	});
};

export default judgeHire;
