/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('panel', { 
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
		started: {
		  type: DataTypes.DATE,
		  allowNull: true
		},
		confirmed: {
		  type: DataTypes.DATE,
		  allowNull: true
		},
		bracket: {
		  type: DataTypes.INTEGER(6),
		  allowNull: false,
		  defaultValue: '0'
		}
	});
};



