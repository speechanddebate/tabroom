/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('entry', { 
		code: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		name: {
			type: DataTypes.STRING(128),
			allowNull: true
		},
		dropped: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		waitlisted: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		dq: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		tba: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		}
	});
};



