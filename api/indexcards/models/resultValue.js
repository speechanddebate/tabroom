/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('resultValue', {
		table_name: 'result_value',
		value: {
			type: DataTypes.TEXT('medium'),
			allowNull: true
		},
		priority: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		}
	});
};

