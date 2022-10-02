const chapterCircuit = (sequelize, DataTypes) => {
	return sequelize.define('chapterCircuit', {
		code: {
			type: DataTypes.STRING(15),
			allowNull: false,
			defaultValue: '',
		},
		full_member: {
			type: DataTypes.BOOLEAN,
			allowNull: false,
			defaultValue: '0',
		},
	},{
		tableName : 'chapter_circuit',
	});
};

export default chapterCircuit;
