/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('ratingTier', {
		table_name: 'rating_tier',
		type : {
			type: DataTypes.ENUM('mpj', 'coach'),
			allowNull: true,
		},
		name: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		description: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		strike: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		conflict: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		min: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
		max: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
		start: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
	});
};
