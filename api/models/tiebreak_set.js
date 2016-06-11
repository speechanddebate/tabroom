/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tiebreak_set', { 
		name: {
			type: DataTypes.STRING,
			allowNull: true
		}
	});
};



