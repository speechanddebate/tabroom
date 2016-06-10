/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {

	return sequelize.define('person', { 

		email: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: '',
			validate: {
				isEmail: true
			},
			unique: true
		},
		first: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		middle: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		last: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		phone: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		alt_phone: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		provider: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		street: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		city: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		state: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		zip: {
			type: DataTypes.STRING(16),
			allowNull: true
		},
		country: {
			type: 'CHAR(4)',
			allowNull: true
		},
		gender: {
			type: 'CHAR(1)',
			allowNull: true
		},
		pronouns: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		ualt_id: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
			unique: true
		},
		site_admin: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		no_email: {
			type: DataTypes.BOOLEAN,
			allowNull: false
		},
		multiple: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		},
		tz: {
			type: DataTypes.STRING,
			allowNull: true
		},
		diversity: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		flags: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



