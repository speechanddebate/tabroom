/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('school_judge', { 
		first: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		last: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		},
		gender: {
			type: DataTypes.CHAR(4),
			allowNull: true
		},
		retired: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0'
		},
		cell: { 
			type: DataTypes.STRING(16),
			allowNull: true
		},
		email: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		diet: {
			type: DataTypes.STRING(64),
			allowNull: true
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		notes: { 
			type: DataTypes.STRING(255),
			allowNull: true
		},
		notes_timestamp: {
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



