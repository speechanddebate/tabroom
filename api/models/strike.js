/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('strike', { 
		type: {
			type: DataTypes.STRING(8),
			allowNull: false,
			defaultValue: ''
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true
		},
		registrant: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		conflictee: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



