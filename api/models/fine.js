/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('fine', { 
		amount: { 
			type: DataTypes.FLOAT,
			allowNull: true
		},
		reason: { 
			type: DataTypes.STRING,
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
		}
	});
};



