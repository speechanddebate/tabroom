/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('person', {
		email: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: '',
			validate: {
				isEmail: true
			},
			unique: true
		},
		first: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		middle: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		last: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		tz: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		phone: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		provider: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		site_admin: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		no_email: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
			unique: true
		}
	});
};



