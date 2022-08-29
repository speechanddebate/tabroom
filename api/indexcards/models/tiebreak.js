const tiebreak = (sequelize, DataTypes) => {
	return sequelize.define('tiebreak', {
		name: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		count: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: '0',
		},
		count_round: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		truncate: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '1',
		},
		truncate_smallest: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: false,
		},
		multiplier: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '1',
		},
		violation: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '1',
		},
		priority: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '0',
		},
		highlow: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		highlow_count: {
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: '0',
		},
	});
};

export default tiebreak;
