/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('shift', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		value_type: {
			type: DataTypes.ENUM("public", "strike", "both"),
			allowNull: false,
			defaultValue: "string"
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true
		},
		fine: { 
			type: DataTypes.FLOAT,
			allowNull: true
		}
	});
};



