/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('school', { 
		name: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: ''
		},
		address: {
			type: DataTypes.STRING,
			allowNull: true
		},
		city: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true
		},
		zip : { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		postal : { 
			type: DataTypes.STRING(15),
			allowNull: true
		},
		country: {
			type: DataTypes.CHAR(4),
			allowNull: true
		},
		coaches: {
			type: DataTypes.STRING,
			allowNull: true
		},
		self_prefs: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		level: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
		naudl: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ipeds: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: '0'
		},
		nces: {
			type         : DataTypes.STRING(15),
			allowNull    : false,
			defaultValue : '0'
		},
		ceeb: {
			type         : DataTypes.STRING(15),
			allowNull    : false,
			defaultValue : '0'
		},
	});
};



