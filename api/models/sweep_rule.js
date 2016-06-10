/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('sweep_rule', { 
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		value: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		place: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};
