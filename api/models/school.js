/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('school', { 
		name: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: ''
		},
		address: {
			type: DataTypes.STRING(255),
			allowNull: true
		},
		city: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		state: {
			type: 'CHAR(4)',
			allowNull: true
		},
		zip : { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		country: {
			type: 'CHAR(4)',
			allowNull: true
		},
		coaches: {
			type: DataTypes.STRING,
			allowNull: true
		},
		level: {
			type: DataTypes.STRING,
			allowNull: true
		},
		naudl: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ipeds: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		nces: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



