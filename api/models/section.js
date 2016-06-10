/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('section', { 
		letter: {
		  type: DataTypes.STRING,
		  allowNull: true
		},
		flight: {
		  type: 'CHAR(1)',
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
		  type: DataTypes.INTEGER(11),
		  allowNull: false,
		  defaultValue: '0'
		}
	});
};



