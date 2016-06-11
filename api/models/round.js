/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('round', { 
		number: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		name: {
			type: DataTypes.STRING,
			allowNull: true
		},
		type: {
			type: DataTypes.STRING,
			allowNull: true
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		post_results: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		flighted: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		start_time: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



