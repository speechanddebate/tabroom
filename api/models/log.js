/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('log', { 
		tableName: "change_log",  // mistakes were made - CLP
		type: { 
			type: DataTypes.STRING(8),
			allowNull: false
		},
		description: {
			type: DataTypes.STRING,
			allowNull: false
		},
		deleted          : {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false
		},
		created : { 
			type         : DataTypes.DATE,
			allowNull    : false
		}
	});
};

