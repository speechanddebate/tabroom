const concessionPurchase = (sequelize, DataTypes) => {
	return sequelize.define('concessionPurchase', {
		quantity: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
		},
		placed: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		fulfilled: {
			type: DataTypes.BOOLEAN,
			allowNull: true,
		},
	},{
		tableName : 'concession_purchase',
	});
};

export default concessionPurchase;
