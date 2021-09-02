/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tournFee', {
		table_name : 'tourn_fee',
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
	});
};
