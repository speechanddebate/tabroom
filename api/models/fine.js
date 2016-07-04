/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('fine', { 
		reason: { 
			type: DataTypes.STRING(63),
			allowNull: true
		},
		amount: { 
			type: DataTypes.FLOAT,
			allowNull: true
		},
		payment: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		deleted: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		deleted_at: { 
			type: DataTypes.DATE,
			allowNull: true
		},
		levied_at: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};


