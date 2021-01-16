
/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('concession_type', { 
		name: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		description : { 
			type: DataTypes.TEXT,
			allowNull: true
		}
	});
};


