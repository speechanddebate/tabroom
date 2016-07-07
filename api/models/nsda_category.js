/* jshint indent: 4 */
module.exports = function(sequelize, DataTypes) {
	return sequelize.define('nsda_category', { 
		name: { 
			type: DataTypes.STRING(31),
			allowNull: true
		},
		type: { 
			type: DataTypes.ENUM('speech', 'debate', 'congress'),
			allowNull: true
		},
		code: { 
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		national: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



