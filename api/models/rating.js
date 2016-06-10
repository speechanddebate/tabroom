/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('rating', { 
		ordinal: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		percentile: { 
			type: DataTypes.FLOAT,
			allowNull: true
		},
		type : {
			type: DataTypes.ENUM('squad', 'entry', 'coach'),
			allowNull: true
		},
		draft : { 
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		sheet : {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



