const score = (sequelize, DataTypes) => {
	return sequelize.define('score', {
		tag: {
			type: DataTypes.STRING,
			allowNull: false,
			defaultValue: '',
		},
		value: {
			type: DataTypes.FLOAT,
			allowNull: true,
		},
		content: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		topic: {
			type: DataTypes.STRING(127),
			allowNull: true,
		},
		speech: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		position: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
	});
};

export default score;
