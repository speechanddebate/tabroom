

/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('rpool_setting', { 
		tag: {
			type: DataTypes.STRING(32),
			allowNull: false
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



