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
			type: DataTypes.SMALLINT,
			allowNull: false,
			defaultValue: '1'
		},
		priority: {
			type: DataTypes.SMALLINT,
			allowNull: false,
			defaultValue: '0'
		},
		highlow: {
			type: DataTypes.TINYINT,
			allowNull: true
		},
		highlow_count: {
			type: DataTypes.TINYINT,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



