const studentVote = (sequelize, DataTypes) => {
	return sequelize.define('studentVote', {
		tag: {
			type         : DataTypes.ENUM('nominee', 'rank'),
			allowNull    : false,
			defaultValue : 'rank',
		},
		value: {
			type: DataTypes.INTEGER,
			allowNull: false,
			defaultValue: '0',
		},
		entered_at : {
			type: DataTypes.DATE,
			allowNull: false,
		},
	},{
		tableName : 'student_vote',
	});
};

export default studentVote;
