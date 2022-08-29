const event = (sequelize, DataTypes) => {
	return sequelize.define('event', {
		name: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		abbr: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		type: {
			type: DataTypes.ENUM,
			values: ['speech','congress','debate','wudc','wsdc'],
			allowNull: false,
		},
		level: {
			type: DataTypes.ENUM,
			values: ['open','jv','novice','champ','es-open','es-novice','middle'],
			allowNull: false,
		},
		fee: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
	});
};

export default event;
