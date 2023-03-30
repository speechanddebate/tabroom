const section = (sequelize, DataTypes) => {
	return sequelize.define('section', {
		letter: {
			type      : DataTypes.STRING(3),
			allowNull : true,
		},
		flight: {
			type      : DataTypes.STRING(3),
			allowNull : true,
		},
		bye: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
		bracket: {
			type         : DataTypes.INTEGER(6),
			allowNull    : false,
			defaultValue : '0',
		},
		publish: {
			type         : DataTypes.BOOLEAN,
			allowNull    : false,
			defaultValue : '0',
		},
	}, {
		tableName: 'panel',
	});
};

export default section;
