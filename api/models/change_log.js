/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('change_log', { 
		type: { 
			type: DataTypes.STRING(8),
			allowNull: false
		},
		description: {
			type: DataTypes.STRING,
			allowNull: false
		},
	});
};



