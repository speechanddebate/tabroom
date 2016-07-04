/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('ballot', { 
		side: {
			type: DataTypes.INTEGER(1),
			allowNull: true
		},
		speakerorder: {
			type: DataTypes.INTEGER(6),
			allowNull: true
		},
		speechnumber: {
			type: DataTypes.INTEGER(6),
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
		seed: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		pullup: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		tv: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		audit: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		collected: {
			type: DataTypes.DATE,
			allowNull: true
		},
		judge_started: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};
