const sweepProtocol = (sequelize, DataTypes) => {
	return sequelize.define('sweepProtocol', {
		table_name: 'sweep_set',
		name: {
			type: DataTypes.STRING,
			allowNull: true,
		},
	});
};

export default sweepProtocol;
