const sweepProtocol = (sequelize, DataTypes) => {
	return sequelize.define('sweepProtocol', {
		name: {
			type: DataTypes.STRING,
			allowNull: true,
		},
	},{
		tableName: 'sweep_set',
	});
};

export default sweepProtocol;
