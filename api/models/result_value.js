/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('result_value', { 
		tag: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: ''
		},
		description: { 
			type: DataTypes.STRING,
			allowNull: true
		}
		value: {
			type: DataTypes.STRING,
			allowNull: true
		},
		priority: {
			type: DataTypes.SMALLINT,
			allowNull: true
		},
		no_sort: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		sort_descending: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};

