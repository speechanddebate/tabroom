/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('double_entry_set', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		type: {
			type: DataTypes.ENUM('none','unlimited','none_in_group','maximum'),
			allowNull: true
		},
		max: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



