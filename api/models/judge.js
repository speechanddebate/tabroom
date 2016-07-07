/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('judge', { 
		code: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		first: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		middle: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		last: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		active: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		obligation: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		hired: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		}
	});
};



