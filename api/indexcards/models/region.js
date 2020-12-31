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
		quota: { 
			type: DataTypes.INTEGER(4),
			allowNull: true
		},
		archdiocese: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		cooke_points: { 
			type: DataTypes.INTEGER,
			allowNull: true
		},
		sweeps: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		}
	});
};



