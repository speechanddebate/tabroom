/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('campusLog', {
		tableName: "campus_log",
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false
		},
		uuid: {
			type: DataTypes.UUID,
			allowNull: false
		},
		description: {
			type: DataTypes.STRING,
			allowNull: false
		},
		deleted          : {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false
		}
	});
};

