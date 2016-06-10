/* jshint indent: 4 */

/* 
 * This is a stub, it permits the relations in index.js to add futher foreign
 * keys, such as regions and membership types where applicable.  An ordinary
 * auto-generated join table will not permit the creation of additional keys 
 */

module.exports = function(sequelize, DataTypes) {
	return sequelize.define('school_circuit', { 
		name: {
			type: DataTypes.STRING(64),
			allowNull: false,
			defaultValue: ''
		}
	});
};


