/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('housing', { 
		type: { 
			type: DataTypes.ENUM('judge', 'student'),
			allowNull: false,
			defaultValue: 'student'
		},
		night: {
			type: DataTypes.DATEONLY,
			allowNull: false,
			defaultValue: '0000-00-00'
		},
		waitlist: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		tba: { 
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: 0
		},
		requested: { 
			type: DataTypes.DATE,
			allowNull: false,
			defaultValue: '0000-00-00 00:00:00'
		}
	});
};



