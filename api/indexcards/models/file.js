/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('file', { 
		tag: {
			type: DataTypes.STRING(128),
			allowNull: true
		},
		type: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		label: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: ''
		},
		filename: { 
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: ''
		},
		published: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		coach: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		uploaded: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



