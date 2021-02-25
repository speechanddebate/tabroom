/* jshint indent: 4 */
module.exports = function(sequelize, DataTypes) {
	return sequelize.define('nsdaCategory', {
		table_name: 'nsda_category',
		name: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		type: {
			type: DataTypes.ENUM('s', 'd', 'c'),
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

