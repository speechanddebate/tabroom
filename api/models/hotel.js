/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('hotel', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		multiple: { 
			type: DataTypes.FLOAT,
			allowNull: true
		}
	});
};



