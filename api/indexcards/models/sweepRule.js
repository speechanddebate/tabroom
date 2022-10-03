const sweepRule = (sequelize, DataTypes) => {
	return sequelize.define('sweepRule', {
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false,
			defaultValue: '',
		},
		value: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		place: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		count: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		rev_min: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		count_round: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		truncate: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	},{
		tableName: 'sweep_rule',
	});
};

export default sweepRule;
