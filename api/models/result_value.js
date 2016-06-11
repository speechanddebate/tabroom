/* jshint indent: 4 */

module.exports = function(sequelize, DataTypes) {
  return sequelize.define('result_value', { 
    tag: {
      type: DataTypes.STRING(32),
      allowNull: false,
      defaultValue: ''
    },
    value: {
      type: DataTypes.STRING,
      allowNull: true
    },
    priority: {
      type: DataTypes.INTEGER(11),
      allowNull: true
    },
    no_sort: {
      type: DataTypes.CHAR(4),
      allowNull: true
	},
    sort_descending: {
      type: DataTypes.CHAR(4),
      allowNull: true
	},
	description: { 
      type: DataTypes.STRING,
      allowNull: true
    }
  });
};

