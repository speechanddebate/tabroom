const round = (sequelize, DataTypes) => {
	return sequelize.define('round', {
		type: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		name: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		label: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		flighted: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		published: {
			type: DataTypes.INTEGER(4),
			allowNull: false,
			defaultValue: '0',
		},
		post_primary     : {
			type         : DataTypes.INTEGER(4),
			allowNull    : true,
		},
		post_secondary     : {
			type         : DataTypes.INTEGER(4),
			allowNull    : true,
		},
		post_feedback     : {
			type         : DataTypes.INTEGER(4),
			allowNull    : true,
		},
		created_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		start_time: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default round;
