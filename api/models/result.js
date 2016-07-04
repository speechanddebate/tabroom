/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('result', { 
		rank: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		percentile: { 
			type: DataTypes.DECIMAL(6,2),
			allowNull: true
		},
		honor: {
			type: DataTypes.STRING,
			allowNull: true
		},
		honor_site: {
			type: DataTypes.STRING,
			allowNull: true
		}
	});
};

