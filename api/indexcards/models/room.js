const room = (sequelize, DataTypes) => {
	return sequelize.define('room', {
		name: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: '',
		},
		quality: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		capacity: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		rowcount: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
		},
		seats: {
			type: DataTypes.INTEGER(4),
			allowNull: true,
		},
		inactive: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		deleted: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		ada: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		notes: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		url: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		judge_url: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		password: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		judge_password: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		api: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
	});
};

export default room;
