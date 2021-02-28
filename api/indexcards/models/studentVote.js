/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('student', {
		tag: {
			type         : DataTypes.ENUM('nominee', 'rank'),
			allowNull    : false,
			defaultValue : 'rank'
		},
		value: {
			type: DataTypes.INTEGER,
			allowNull: false,
			defaultValue: '0'
		},
		entered_at : {
			type: DataTypes.DATE,
			allowNull: false
		}
	});
};

