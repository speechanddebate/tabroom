
/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('concessionPurchase', {
		table_name : 'concession_purchase',
		quantity: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		placed: {
			type: DataTypes.DATE,
			allowNull: true
		},
		fulfilled: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		}
	});
};



