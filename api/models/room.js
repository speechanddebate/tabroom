/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('room', { 
		building: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		quality: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		capacity: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		inactive: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		notes: { 
			type: DataTypes.STRING(63),
			allowNull: true
		}
	});
};



