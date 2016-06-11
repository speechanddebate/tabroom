/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('setting_label', { 
		lang: { 
			type: DataTypes.CHAR(2),
			allowNull: false
		},
		label: {
			type: DataTypes.STRING(127),
			allowNull: false
		},
		guide: { 
			type: DataTypes.TEXT,
			allowNull: true
		},
		options: { 
			type: DataTypes.TEXT,
			allowNull: true
		}
	});
};



