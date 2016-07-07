/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('event', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		type: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		abbr: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		fee: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true
		}
	});
};



