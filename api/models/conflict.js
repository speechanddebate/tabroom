/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('conflict', { 
		type: {
			type: DataTypes.STRING(8),
			allowNull: false,
			defaultValue: ''
		}
	});
};



