
/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('result_set', { 
    label: {
      type: DataTypes.STRING(64),
      allowNull: true
    },
	bracket: {
      type: DataTypes.BOOLEAN,
      allowNull: true
	},
	published: {
      type: DataTypes.BOOLEAN,
      allowNull: true
	}
  });
};

