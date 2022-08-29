const strike = (sequelize, DataTypes) => {
	return sequelize.define('strike', {
		type: {
			type: DataTypes.STRING(31),
			allowNull: false,
			defaultValue: '',
		},
		start: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		registrant: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		conflict: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		conflictee: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		dioregion: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		created_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default strike;
