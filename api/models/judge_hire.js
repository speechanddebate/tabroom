/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('judge_hire', { 
		entries_requested: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		entries_accepted: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		rounds_requested: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		rounds_accepted: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		requested_at: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};


