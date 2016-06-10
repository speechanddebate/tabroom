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
		hidden: { 
			type: DataTypes.BOOLEAN,
			allowNull: true
		}
	});
};



