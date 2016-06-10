/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tourn', { 
		name: {
			type: DataTypes.STRING(64),
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
		reg_start: {
			type: DataTypes.DATE,
			allowNull: true
		},
		reg_end: {
			type: DataTypes.DATE,
			allowNull: true
		},
		hidden: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		webname: {
			type: DataTypes.STRING(32),
			allowNull: true
		},
		tz: {
			type: DataTypes.STRING(32),
			allowNull: true
		},
		city: {
			type: DataTypes.STRING,
			allowNull: true
		},
		state: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		country: {
			type: DataTypes.STRING(16),
			allowNull: true
		}
	});
};



