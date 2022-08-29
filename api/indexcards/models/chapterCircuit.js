const chapterCircuit = (sequelize, DataTypes) => {
	return sequelize.define('chapterCircuit', {
		table_name : 'chapter_circuit',
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
	});
};

export default chapterCircuit;
