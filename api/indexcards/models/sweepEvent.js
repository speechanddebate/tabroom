const sweepEvent = (sequelize, DataTypes) => {
	return sequelize.define('sweepEvent', {
		table: 'sweep_event',
		event_type: {
			type: DataTypes.ENUM('all','congress','debate','speech','wsdc','wudc'),
			allowNull: true,
		},
		event_level: {
			type: DataTypes.ENUM('all','open','jv','novice','champ','es-open','es-novice','middle'),
			allowNull: true,
		},
	});
};

export default sweepEvent;
