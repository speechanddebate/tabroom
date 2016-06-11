/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('room', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		building: { 
			type: DataTypes.STRING(32),
			allowNull: true
		},
		quality: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		capacity: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		inactive: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '1'
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



