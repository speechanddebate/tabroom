/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('diocese', { 
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		code: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true
		},
		quota: { 
			type: DataTypes.TINYINT,
			allowNull: true
		},
		active: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		archdiocese: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		cooke_award_points: { 
			type: DataTypes.INTEGER,
			allowNull: true
		},
		sweeps: { 
			type: DataTypes.SMALLINT,
			allowNull: true
		}
	});
};



