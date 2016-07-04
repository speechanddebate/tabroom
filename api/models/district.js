/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('district', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		code: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		location: { 
			type: DataTypes.STRING(63),
			allowNull: true
		}
	});
};



