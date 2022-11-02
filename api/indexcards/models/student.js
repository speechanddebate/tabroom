const student = (sequelize, DataTypes) => {
	return sequelize.define('student', {
		first: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		middle: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		last: {
			type: DataTypes.STRING(63),
			allowNull: false,
			defaultValue: '',
		},
		phonetic: {
			type: DataTypes.STRING(63),
			allowNull: true,
		},
		grad_year: {
			type: DataTypes.INTEGER(6),
			allowNull: false,
			defaultValue: '2010',
		},
		novice: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		retired: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
		gender: {
			type: DataTypes.CHAR(4),
			allowNull: true,
		},
		nsda: {
			type: DataTypes.INTEGER(11),
			allowNull: true,
		},
	});
};

export default student;
