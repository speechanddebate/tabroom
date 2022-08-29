const email = (sequelize, DataTypes) => {
	return sequelize.define('email', {
		subject: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: '',
		},
		content: {
			type: DataTypes.TEXT('medium'),
			allowNull: false,
		},
		metadata: {
			type: DataTypes.TEXT('medium'),
			allowNull: false,
		},
		hidden: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: false,
		},
		sent_to: {
			type: DataTypes.STRING(128),
			allowNull: false,
			defaultValue: '',
		},
		sent_at: {
			type: DataTypes.DATE,
			allowNull: true,
		},
	});
};

export default email;
