/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('section', { 
		tableName : 'panel',    //Mistakes were made --CLP
		letter: {
		  type: DataTypes.STRING,
		  allowNull: true
		},
		flight: {
		  type: DataTypes.STRING(3),
		  allowNull: true
		},
		bye: {
		  type: DataTypes.BOOLEAN,
		  allowNull: false,
		  defaultValue: '0'
		},
		bracket: {
		  type: DataTypes.INTEGER(6),
		  allowNull: false,
		  defaultValue: '0'
		}
	});
};



