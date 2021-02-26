/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('resultKey', {
		table_name : "result_key",
		tag: {
			type      : DataTypes.STRING(63),
			allowNull : true
		},
		description: {
			type         : DataTypes.STRING(255),
			allowNull    : true
		},
		no_sort: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0'
		},
		sort_desc: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0'
		}
	});
};

