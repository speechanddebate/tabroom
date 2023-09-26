const resultSet = (sequelize, DataTypes) => {
	return sequelize.define('resultSet', {
		tag : {
			type         : DataTypes.ENUM('entry', 'student', 'chapter'),
			allowNull    : false,
			defaultValue : 'entry',
		},
		label: {
			type      : DataTypes.STRING(64),
			allowNull : true,
		},
		code: {
			type      : DataTypes.STRING(15),
			allowNull : true,
		},
		bracket: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		coach: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		generated: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	},{
		tableName: 'result_set',
	});
};

export default resultSet;
