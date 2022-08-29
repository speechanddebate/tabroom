const campusLog = (sequelize, DataTypes) => {
	return sequelize.define('campusLog', {
		tag: {
			type: DataTypes.STRING(31),
			allowNull: false,
		},
		uuid: {
			type: DataTypes.UUID,
			allowNull: true,
		},
		description: {
			type: DataTypes.STRING,
			allowNull: false,
		},
		timestamp: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	},{
		tableName: 'campus_log',
	});
};

export default campusLog;
