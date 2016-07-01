/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('room_strike', { 
		type: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: 'time'
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



