/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('region', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		code: {
			type: DataTypes.STRING(8),
			allowNull: true
		},
		active: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		ncfl_quota: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		ncfl_archdiocese: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		ncfl_cooke: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		ncfl_sweeps: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		}
	});
};



