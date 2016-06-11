/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('student', { 
		first: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: ''
		},
		middle: { 
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: ''
		},
		last: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: ''
		},
		grad_year: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
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
			type: 'CHAR(1)',
			allowNull: true
		},
		phonetic: {
			type: DataTypes.STRING,
			allowNull: true
		},
		ualt_id: {
			type: DataTypes.INTEGER(11),
			allowNull: true
		}
	});
};



