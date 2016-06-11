/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('event', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		type: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		abbr: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		fee: {
			type: DataTypes.FLOAT,
			allowNull: true
		}
	});
};



