/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('qualifier', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false
		},
		result: {
			type: DataTypes.STRING(127),
			allowNull: true
		}
	});
};




