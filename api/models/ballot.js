/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('ballot', { 
		side: {
			type: DataTypes.INTEGER(4),
			allowNull: true
		},
		bye: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		forfeit: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		chair: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		speaker_order: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		speech_number: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		collected_at: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};
