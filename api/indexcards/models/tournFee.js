const tournFee = (sequelize, DataTypes) => {
	return sequelize.define('tournFee', {
		amount: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
		reason: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	},{
		tableName : 'tourn_fee',
	});
};

export default tournFee;
