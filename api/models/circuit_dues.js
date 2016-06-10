/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('circuit_dues', { 
		amount: { 
			type: DataTypes.FLOAT,
			allowNull: true
		},
		paid_on: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



