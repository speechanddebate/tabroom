/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('round', { 
		type: {
			type: DataTypes.STRING(15),
			allowNull: true
		},
		name: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		label: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		flighted: {
			type: DataTypes.INTEGER(4),
			allowNull: true
		},
		published: {
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: '0'
		},
		post_results     : {
			type         : DataTypes.INTEGER(4),
			allowNull    : false,
			defaultValue : '0'
		},
		created: {
			type: DataTypes.DATE,
			allowNull: true
		},
		start_time: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};


