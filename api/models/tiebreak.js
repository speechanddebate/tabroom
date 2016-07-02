/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tiebreak', { 
		name: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		count: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: '0'
		},
		multiplier: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '1'
		},
		priority: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '0'
		},
		highlow: {
			type: DataTypes.INTEGER(4),
			allowNull: true
		},
		highlow_count: {
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: '0'
		}
	});
};



