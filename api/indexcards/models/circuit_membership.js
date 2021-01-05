/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('circuit_membership', { 
		name: {
			type         : DataTypes.STRING(64),
			allowNull    : false,
			defaultValue : ''
		},
		approval: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0'
		},
		description: {
			type         : DataTypes.STRING(128),
			allowNull    : true,
			defaultValue : '0'
		}
	});
};

