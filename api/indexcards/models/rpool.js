/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('rpool', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: false
		}
	});
};



