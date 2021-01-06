/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('chapter_judge', { 
		first: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: ''
		},
		middle: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: ''
		},
		last: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: ''
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		retired: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0
		},
		phone: { 
			type: DataTypes.STRING(31),
			allowNull: true
		},
		email: { 
			type: DataTypes.STRING(127),
			allowNull: true
		},
		diet: {
			type: DataTypes.STRING,
			allowNull: true
		},
		notes: { 
			type: DataTypes.STRING(255),
			allowNull: true
		},
		notes_timestamp: {
			type: DataTypes.DATE,
			allowNull: true
		},
		gender: {
			type: DataTypes.CHAR(4),
			allowNull: true
		}
	});
};



