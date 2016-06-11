/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('judge_hire', { 
		requested_at: {
			type: DataTypes.DATE,
			allowNull: true
		},
		entries_requested: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		entries_accepted: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		rounds_requested: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		rounds_accepted: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};


