/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('setting', { 
		type: { 
			type: DataTypes.STRING(31),
			allowNull: false
		},
		subtype: { 
			type: DataTypes.STRING(31),
			allowNull: false
		},
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false
		},
		value_type: {
			type: DataTypes.ENUM("text", "string", "bool", "integer", "decimal", "datetime", "enum"),
			allowNull: false,
			defaultValue: "string"
		},
		conditions: { 
			type: DataTypes.TEXT,
			allowNull: true
		}
	});
};

