/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('webpage', { 
		title: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		content: { 
			type: DataTypes.TEXT,
			allowNull: true
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		sitewide: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		page_order: { 
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



