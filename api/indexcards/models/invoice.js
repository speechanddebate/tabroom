const invoice = (sequelize, DataTypes) => {
	return sequelize.define('invoice', {
		blusynergy: {
			type: DataTypes.INTEGER,
			allowNull: true,
		},
		blu_number: {
			type: DataTypes.STRING(31),
			allowNull: true,
		},
		total: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: false,
			defaultValue: '0.00',
		},
		paid: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		details: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
	});
};

export default invoice;
