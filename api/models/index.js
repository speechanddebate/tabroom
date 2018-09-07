"use strict";

const fs        = require("fs");
const path      = require("path");
const Sequelize = require("sequelize");

const basename = path.basename(module.filename);
const env      = process.env.NODE_ENV || "development";
const config   = require(__dirname + '/../config/config.json')[env];

const sequelize = new Sequelize(
	config.database, 
	config.username, 
	config.password, 
	config.sequelizeOptions
);

const db = {};

// This will read in all the model definitions in the /model directory and hook
// them in as sequelize objects populated into the db object

fs
	.readdirSync(__dirname)
	.filter(function(file) {
		return (file.indexOf(".") !== 0) && (file !== basename);
	})
	.forEach(function(file) {
		const model = sequelize["import"](path.join(__dirname, file));
		db[model.name] = model;
	});

Object.keys(db).forEach(function(modelName) {
	if ("associate" in db[modelName]) {
		db[modelName].associate(db);
	}
});

// Initialize the data objects.

db.sequelize = sequelize;
db.Sequelize = Sequelize;

module.exports = db;

//db.Ballot.belongsTo(db.Person, {as: "CollectedBy"});

