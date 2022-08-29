const webpage = (sequelize, DataTypes) => {
	return sequelize.define('webpage', {
		title: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		content: {
			type: DataTypes.TEXT,
			allowNull: true,
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		sitewide: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		special: {
			type: DataTypes.STRING(15),
			allowNull: true,
		},
		page_order: {
			type: DataTypes.INTEGER(6),
			allowNull: true,
		},
	});
};

export default webpage;
