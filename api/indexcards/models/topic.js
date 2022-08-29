const topic = (sequelize, DataTypes) => {
	return sequelize.define('topic', {
		tag: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		source: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		event_type: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		topic_text: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		school_year: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		sort_order: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		created_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default topic;
