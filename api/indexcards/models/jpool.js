/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('jpool', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false
		}
	});
};



