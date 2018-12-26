/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('permission', { 
		tag: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		details: {
			type: DataTypes.JSON,
			allowNull: true
		}
	});

};

