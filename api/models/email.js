/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('email', { 
		subject: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: ''
		},
		content: { 
			type: DataTypes.TEXT,
			allowNull: false
		},
		sent_to: { 
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: ''
		},
		sent_at: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



