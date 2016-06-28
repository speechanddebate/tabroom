/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('concession', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		price: {
			type: DataTypes.DECIMAL(6,2),
			allowNull: true
		},
		description: { 
			type: DataTypes.STRING,
			allowNull: true
		},
		cap: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		school_cap: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		deadline: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



