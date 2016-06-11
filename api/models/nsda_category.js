/* jshint indent: 4 */
module.exports = function(sequelize, DataTypes) {
	return sequelize.define('nsda_category', { 
		label: { 
			type: DataTypes.STRING(127),
			allowNull: true
		},
		type: { 
			type: DataTypes.ENUM('speech', 'debate', 'congress'),
			allowNull: true
		},
		national: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		}
	});
};



