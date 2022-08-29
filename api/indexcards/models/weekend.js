const weekend = (sequelize, DataTypes) => {
	return sequelize.define('weekend', {
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: '',
		},
		start : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		end : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		reg_start : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		reg_end : {
			type: DataTypes.DATE,
			allowNull: true,
		},

		freeze_deadline : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		drop_deadline : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		judge_deadline : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		fine_deadline : {
			type: DataTypes.DATE,
			allowNull: true,
		},
		city: {
			type: DataTypes.STRING(127),
			allowNull: true,
		},
		state: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
	});
};

export default weekend;
