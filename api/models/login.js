/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('login', { 
		username: {
			type: DataTypes.STRING(64),
			allowNull: false,
			unique: true
		},
		password: {
			type: DataTypes.STRING(128),
			allowNull: true
		},
		sha512: {
			type: DataTypes.STRING(128),
			allowNull: true
		},
		spinhash: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		accesses: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		last_access: {
			type: DataTypes.DATE,
			allowNull: true
		},
		pass_timestamp: {
			type: DataTypes.DATE,
			allowNull: true
		},
		pass_changekey: {
			type: DataTypes.STRING(127),
			allowNull: true
		},
		pass_change_expires: {
			type: DataTypes.DATE,
			allowNull: true
		},
		source: {
			type: 'CHAR(4)',
			allowNull: true
		},
		ualt_id: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};




