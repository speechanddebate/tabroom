const file = (sequelize, DataTypes) => {
	return sequelize.define('file', {
		tag: {
			type: DataTypes.STRING(128),
			allowNull: true,
		},
		type: {
			type: DataTypes.STRING(16),
			allowNull: true,
		},
		label: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: '',
		},
		filename: {
			type: DataTypes.STRING(127),
			allowNull: false,
			defaultValue: '',
		},
		published: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0,
		},
		page_order: {
			type: DataTypes.SMALLINT,
			allowNull: true,
		},
		coach: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: 0,
		},
		uploaded: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default file;
