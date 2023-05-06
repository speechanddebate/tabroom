const ballot = (sequelize, DataTypes) => {
	return sequelize.define('ballot', {
		id : {
			type          : DataTypes.INTEGER,
			autoIncrement : true,
			primaryKey    : true,
		},
		side: {
			type: DataTypes.INTEGER(1),
			allowNull: true,
		},
		speakerorder: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		seat: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
		bye: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		forfeit: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		tv: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		audit: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		chair: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		judge_started: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default ballot;
