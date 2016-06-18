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
		speakerorder: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		speechnumber: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		pullup: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		seed: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		judge_started: { 
			type: DataTypes.DATE,
			allowNull: true
		},
		collected: {
			type: DataTypes.DATE,
			allowNull: true
		},
		entered: {
			type: DataTypes.DATE,
			allowNull: true
		},
		audited: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};
