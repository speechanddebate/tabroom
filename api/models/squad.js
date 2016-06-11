/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('squad', { 
		name: {
			type: DataTypes.STRING,
			allowNull: true
		},
		code: {
			type: DataTypes.STRING,
			allowNull: true
		},
		onsite: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		onsite_at: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



