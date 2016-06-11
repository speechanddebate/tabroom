/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('housing_slots', { 
		night: {
			type: DataTypes.DATEONLY,
			allowNull: false,
			defaultValue: '0000-00-00'
		},
		slots: { 
			type: DataTypes.INTEGER(8),
			allowNull: true
		}
	});
};



