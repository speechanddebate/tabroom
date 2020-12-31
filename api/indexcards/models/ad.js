/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('ad', { 
		tag: {
			type         : DataTypes.STRING(127),
			allowNull    : false,
			defaultValue : ''
		},
		filename      : {
			type      : DataTypes.STRING,
			allowNull : true
		},
		url           : {
			type      : DataTypes.STRING,
			allowNull : true
		},
		sort_order    : {
			type      : DataTypes.INTEGER(6),
			allowNull : true
		},
		start         : {
			type      : DataTypes.DATE,
			allowNull : true
		},
		end           : {
			type      : DataTypes.DATE,
			allowNull : true
		},
		approved         : {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : false
		}
	});
};

