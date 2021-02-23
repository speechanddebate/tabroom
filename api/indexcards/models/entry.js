/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('entry', {
		code: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		name: {
			type: DataTypes.STRING(128),
			allowNull: true
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		active: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		tba: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		dropped: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		waitlist: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		unconfirmed: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		dq: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		created_at: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



