/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('squad_judge', { 
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
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: true
		},
		diet: { 
			type: DataTypes.STRING,
			allowNull: true
		}, 
		notes: { 
			type: DataTypes.STRING,
			allowNull: true
		}, 
		notes_timestamp: { 
			type: DataTypes.DATE,
			allowNull: true
		}
	});
};



