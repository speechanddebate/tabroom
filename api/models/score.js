/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('score', { 
		tag: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: ''
		},
		value: {
			type: DataTypes.FLOAT,
			allowNull: true
		},
		content: {
			type: DataTypes.TEXT,
			allowNull: true
		},
		speech: {
			type: DataTypes.INTEGER(4),
			allowNull: true
		},
		position: {
			type: DataTypes.INTEGER(4),
			allowNull: true
		}
	});
};

