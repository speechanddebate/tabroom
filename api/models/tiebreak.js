/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tiebreak', { 
		name: {
			type: DataTypes.STRING,
			allowNull: true
		},
		count: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: '0'
		},
		multiplier: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '1'
		},
		priority: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		highlow: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		highlow_count: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		}
	});
};



