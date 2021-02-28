
/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('sweepSet', {
		table_name: 'sweep_set',
		name: {
			type: DataTypes.STRING,
			allowNull: true
		}
	});
};



