/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('campusLog', {
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false,
		},
		uuid: {
			type: DataTypes.UUID,
			allowNull: true,
		},
		description: {
			type: DataTypes.STRING,
			allowNull: false,
		},
	},{
		tableName: 'campus_log',
	});
};
