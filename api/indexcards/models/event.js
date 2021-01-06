/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('event', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		abbr: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		type: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		fee: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true
		}
	});
};



