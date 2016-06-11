/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('rating_tier', { 
		name: {
		  type: DataTypes.STRING(32),
		  allowNull: true
		},
		type : {
			type: DataTypes.ENUM('mpj', 'coach'),
			allowNull: true
		},
		description: {
		  type: DataTypes.STRING,
		  allowNull: true
		},
		strike: { 
		  type: DataTypes.BOOLEAN,
		  allowNull: true
		},
		conflict: { 
		  type: DataTypes.BOOLEAN,
		  allowNull: true
		},
		min: { 
		  type: DataTypes.INTEGER(4),
		  allowNull: true
		},
		max: { 
		  type: DataTypes.INTEGER(4),
		  allowNull: true
		},
		default_tier: { 
		  type: DataTypes.BOOLEAN,
		  allowNull: true
		}
	});
};



