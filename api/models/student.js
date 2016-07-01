/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('student', { 
		first: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		middle: { 
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		last: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		phonetic: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		grad_year: {
			type: DataTypes.SMALLINT,
			allowNull: false,
			defaultValue: '2010'
		},
		novice: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		retired: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		gender: {
			type: DataTypes.CHAR(1),
			allowNull: true
		},
		diet: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		birthdate: { 
			type: DataTypes.DATE,
			allowNull: true
		},
		school_sid: {
			type: DataTypes.STRING(63),
			allowNull: true
		},
		race: {
			type: DataTypes.STRING(31),
			allowNull: true
		},
		ualt_id: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};


