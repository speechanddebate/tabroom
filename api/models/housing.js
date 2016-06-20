/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('housing_request', { 
		night: {
			type: DataTypes.DATEONLY,
			allowNull: false,
			defaultValue: '0000-00-00'
		},
		type: { 
			type: DataTypes.ENUM('judge', 'student'),
			allowNull: false,
			defaultValue: 'student'
		},
		tba: { 
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: 0
		},
		waitlist: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		}
	});
};



