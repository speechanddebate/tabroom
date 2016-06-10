/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('qualifier', { 
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false
		},
		value: {
			type: DataTypes.STRING(64),
			allowNull: true
		}
	});
};




