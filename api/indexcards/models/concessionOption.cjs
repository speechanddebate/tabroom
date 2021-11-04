/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('concessionOption', {
		table_name : 'concession_option',
		name: {
			type: DataTypes.STRING(8),
			allowNull: true,
		},
		description : {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		disabled : {
			type: DataTypes.BOOLEAN,
			allowNull: true,
		},
	});
};
