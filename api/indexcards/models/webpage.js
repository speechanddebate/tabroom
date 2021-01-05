/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('webpage', { 
		title: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		content: { 
			type: DataTypes.TEXT,
			allowNull: true
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		sitewide: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		page_order: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		}
	});
};



