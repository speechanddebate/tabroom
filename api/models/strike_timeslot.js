/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('strike_timeslot', { 
		name: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
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



