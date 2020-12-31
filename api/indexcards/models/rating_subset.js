/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('rating_subset', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: true
		}
	});
};



