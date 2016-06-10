/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('follower', { 
		cell: {
			type: DataTypes.STRING(32),
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




