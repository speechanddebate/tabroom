/* jshint indent: 4 */
module.exports = function(sequelize, DataTypes) {
	return sequelize.define('nsda_honor', { 
		tag: { 
			type: DataTypes.STRING(63),
			allowNull: true
		},
		value: {
			type: DataTypes.INTEGER(11),
			allowNull: false,
			defaultValue: '0'
		},
		earned_at: { 
			type: DataTypes.DATE,
			allowNull: true
		},
		venue: { 
			type: DataTypes.STRING(127),
			allowNull: true
		},
		event_name: { 
			type: DataTypes.STRING(63),
			allowNull: true
		},
		level: { 
			type: DataTypes.ENUM('hs', 'college', 'middle', 'elem'),
			allowNull: true
		},
		source: { 
			type: DataTypes.ENUM('T', 'J', 'M', 'O'),
			allowNull: true
		},
		results: { 
			type: DataTypes.TEXT,
			allowNull: true
		}
	});
};



