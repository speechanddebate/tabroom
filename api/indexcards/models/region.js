/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('region', {
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: '',
		},
		code: {
			type: DataTypes.STRING(8),
			allowNull: true,
		},
	});
};
