/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('file', { 
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
		posting: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		result: { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		published: { 
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



