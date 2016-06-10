/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('tourn_fee', { 
		amount: { 
			type: DataTypes.FLOAT,
			allowNull: true
		},
		reason: { 
			type: DataTypes.STRING,
			allowNull: true
		},
		start: { 
			type: DataTypes.DATE,
			allowNull: true
		},
		end: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



