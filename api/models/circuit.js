/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('circuit', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		abbr: {
			type: DataTypes.STRING(8),
			allowNull: true
		},
		active: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		state: { 
			type: DataTypes.STRING(8),
			allowNull: true
		},
		country: {
			type: DataTypes.STRING(8),
			allowNull: true
		},
		tz: { 
			type: DataTypes.STRING(32),
			allowNull: true
		},
		url: { 
			type: DataTypes.STRING(64),
			allowNull: true
		},
		webname: { 
			type: DataTypes.STRING(32),
			allowNull: true
		}
	});
};



