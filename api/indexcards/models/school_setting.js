

/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('school_setting', { 
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false,
			defaultValue: ''
		},
		value: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		value_text: {
			type: DataTypes.TEXT,
			allowNull: true
		},
		value_date: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



