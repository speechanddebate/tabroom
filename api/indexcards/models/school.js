/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('school', { 
		name: {
			type: DataTypes.STRING(127),
			allowNull: true
		},
		code: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		onsite: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



