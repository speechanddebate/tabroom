const timeslot = (sequelize, DataTypes) => {
	return sequelize.define('timeslot', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default timeslot;
