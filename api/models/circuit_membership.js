/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('circuit_membership', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		approval_required: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		region_required: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		description: { 
			type: DataTypes.STRING,
			allowNull: true
		}
	});
};



