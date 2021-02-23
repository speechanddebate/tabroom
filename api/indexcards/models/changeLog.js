/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('changeLog', {
		table_name : "change_log",
		tag: {
			type: DataTypes.STRING(63),
			allowNull: false
		},
		description: {
			type: DataTypes.TEXT("medium"),
			allowNull: false
		},
		deleted          : {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false
		},
		created_at : {
			type         : DataTypes.DATE,
			allowNull    : false
		}
	});
};

