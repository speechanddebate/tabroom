/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('setting', { 
		type: { 
			type: DataTypes.STRING(31),
			allowNull: false
		},
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false
		},
		category: { 
			type: DataTypes.STRING(31),
			allowNull: false
		},
		value_type: {
			type: DataTypes.ENUM("text", "string", "bool", "integer", "float", "datetime", "enum"),
			allowNull: false,
			defaultValue: "string"
		},
		conditions: { 
			type: DataTypes.TEXT,
			allowNull: true
		}
	});
};



