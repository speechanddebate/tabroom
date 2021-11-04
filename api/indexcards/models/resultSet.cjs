/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('resultSet', {
		table_name: 'result_set',
		tag : {
			type         : DataTypes.ENUM('entry', 'student', 'chapter'),
			allowNull    : false,
			defaultValue : 'entry',
		},
		label: {
			type      : DataTypes.STRING(64),
			allowNull : true,
		},
		bracket: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		coach: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		generated: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};
