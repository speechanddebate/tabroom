/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('result', { 
		honor: {
			type: DataTypes.STRING,
			allowNull: true
		},
		honor_site: {
			type: DataTypes.STRING,
			allowNull: true
		},
		rank: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		percentile: { 
			type: DataTypes.FLOAT,
			allowNull: true
		}
	});
};

