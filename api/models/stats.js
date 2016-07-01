/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('stats', { 
		type: { 
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: ''
		},
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false,
			defaultValue: ''
		},
		value: {
			type: DataTypes.DECIMAL(8,2), 
			allowNull: true
		}
	});
};



