/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('judge', { 
		first: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		last: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		code: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		active: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		obligation_rounds: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		hired_rounds: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



