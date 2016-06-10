/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('stat', { 
		type: { 
			type: DataTypes.STRING(16),
			allowNull: false,
			defaultValue: ''
		},
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		value: {
			type: DataTypes.STRING(64),
			allowNull: true
		}
	});
};



