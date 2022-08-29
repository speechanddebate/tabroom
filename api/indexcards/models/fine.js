const fine = (sequelize, DataTypes) => {
	return sequelize.define('fine', {
		reason: {
			type: DataTypes.STRING(255),
			allowNull: true,
		},
		amount: {
			type: DataTypes.DECIMAL(8,2),
			allowNull: true,
		},
		payment: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		levied_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
		deleted: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		deleted_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default fine;
