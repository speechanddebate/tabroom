/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('follower', { 
		type: {
			type: DataTypes.STRING(8),
			allowNull: false
		},
		cell: {
			type: DataTypes.INTEGER(20),
			allowNull: false
		},
		email: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		domain: {
			type: DataTypes.STRING(64),
			allowNull: true
		}
	});
};




