/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('site', { 
		name: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: ''
		},
		directions: {
			type: DataTypes.TEXT,
			allowNull: true
		},
		dropoff: {
			type: DataTypes.STRING,
			allowNull: true
		}
	});
};

