/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('chapter', { 
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
		level: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		naudl: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



