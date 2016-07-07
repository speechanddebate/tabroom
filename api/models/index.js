"use strict";

var fs        = require("fs");
var path      = require("path");
var Sequelize = require("sequelize");

var basename = path.basename(module.filename);
var env      = process.env.NODE_ENV || "development";
var config   = require(__dirname + '/../config/config.json')[env];

var sequelize = new Sequelize(config.database, config.username, config.password, config);

var db       = {};

// This will read in all the model definitions in the /model directory and hook
// them in as sequelize objects populated into the db object

fs
	.readdirSync(__dirname)
	.filter(function(file) {
		return (file.indexOf(".") !== 0) && (file !== basename);
	})
	.forEach(function(file) {
		var model = sequelize["import"](path.join(__dirname, file));
		db[model.name] = model;
	});

Object.keys(db).forEach(function(modelName) {
	if ("associate" in db[modelName]) {
		db[modelName].associate(db);
	}
});

db.calendar.belongsTo(db.tourn);
db.calendar.belongsTo(db.person);

db.change_log.belongsTo(db.person);
db.change_log.belongsTo(db.tourn);
db.change_log.belongsTo(db.event);
db.change_log.belongsTo(db.school);
db.change_log.belongsTo(db.entry);
db.change_log.belongsTo(db.judge);
db.change_log.belongsTo(db.panel, {as: "old_panel"});
db.change_log.belongsTo(db.panel, {as: "new_panel"});
db.change_log.belongsTo(db.fine);

db.circuit.belongsToMany(db.tourn, {through: 'tourn_circuits'});
db.circuit.belongsToMany(db.person, {through: 'permissions'});

db.circuit.hasMany(db.circuit_membership);
db.circuit.hasMany(db.circuit_setting);
db.circuit.hasMany(db.file);

db.circuit_membership.belongsTo(db.circuit);

db.site.belongsTo(db.person, {as: 'host'});
db.site.belongsTo(db.circuit);
db.site.hasMany(db.room);
db.site.hasMany(db.round);
db.site.belongsToMany(db.tourn, {through: 'tourn_sites'});

db.room.belongsTo(db.site);
db.room.belongsToMany(db.rpool, {through: 'pool_rooms'});
db.room.hasMany(db.room_strike);

db.room_strike.belongsTo(db.room);
db.room_strike.belongsTo(db.event);
db.room_strike.belongsTo(db.tourn);
db.room_strike.belongsTo(db.entry);
db.room_strike.belongsTo(db.judge);

db.chapter.hasMany(db.student);
db.chapter.hasMany(db.chapter_judge);
db.chapter.hasMany(db.school);
db.chapter.belongsToMany(db.person, {through: 'permissions'});

db.chapter_circuit.belongsTo(db.circuit);
db.chapter_circuit.belongsTo(db.chapter);
db.chapter_circuit.belongsTo(db.region);
db.chapter_circuit.belongsTo(db.circuit_membership);

db.chapter_judge.belongsTo(db.chapter);
db.chapter_judge.belongsTo(db.person);
db.chapter_judge.belongsTo(db.person, {as: "person_request"});
db.chapter_judge.hasMany(db.judge);

db.circuit.belongsToMany(db.chapter, {through: 'chapter_circuits'});
db.chapter.belongsToMany(db.circuit, {through: 'chapter_circuits'});

db.district.belongsTo(db.person, {as: "chair"});

db.student.belongsTo(db.chapter);
db.student.belongsTo(db.person);
db.student.belongsTo(db.person, {as: "person_request"});
db.student.belongsToMany(db.entry, {through: 'entry_students'});
db.student.hasMany(db.score);

db.region.belongsTo(db.circuit);
db.region.belongsTo(db.tourn);

db.region.hasMany(db.strike);
db.region.hasMany(db.fine);
db.region.belongsToMany(db.person, {through: 'permissions'});

db.region.belongsToMany(db.chapter, {through: 'chapter_circuits'});
db.chapter.belongsToMany(db.region, {through: 'chapter_circuits'});

// Tournament wide relations

db.tourn.hasOne(db.calendar);

db.tourn.hasMany(db.tourn_setting);

db.tourn.hasMany(db.change_log);
db.tourn.hasMany(db.category);
db.tourn.hasMany(db.concession);
db.tourn.hasMany(db.email);
db.tourn.hasMany(db.event);
db.tourn.hasMany(db.file);
db.tourn.hasMany(db.fine);
db.tourn.hasMany(db.follower);
db.tourn.hasMany(db.hotel);
db.tourn.hasMany(db.region);
db.tourn.hasMany(db.result_set);
db.tourn.hasMany(db.school);
db.tourn.hasMany(db.timeslot);
db.tourn.hasMany(db.tourn_fee);
db.tourn.hasMany(db.tiebreak_set);
db.tourn.hasMany(db.webpage);

db.tourn.belongsToMany(db.site, {through: 'tourn_sites'});
db.tourn.belongsToMany(db.person, {through: 'permissions'});
db.tourn.belongsToMany(db.circuit, {through: 'tourn_circuits'});
db.tourn.belongsToMany(db.person, {through: 'tourn_ignore'});

db.tourn_fee.belongsTo(db.tourn);

db.timeslot.belongsTo(db.tourn);
db.timeslot.hasMany(db.round);
db.timeslot.hasMany(db.strike);

db.category.belongsTo(db.tourn);
db.category.hasMany(db.event);
db.category.hasMany(db.judge);
db.category.hasMany(db.category_setting);
db.category.hasMany(db.change_log);

db.event.belongsTo(db.tourn);
db.event.belongsTo(db.category);
db.event.belongsTo(db.pattern);
db.event.belongsTo(db.rating_subset);

db.event.hasMany(db.file);
db.event.hasMany(db.stats);
db.event.hasMany(db.entry);
db.event.hasMany(db.round);
db.event.hasMany(db.qualifier);
db.event.hasMany(db.event_setting);
db.event.hasMany(db.change_log);
db.event.belongsToMany(db.sweep_set, {through: 'sweep_events'});

db.pattern.belongsTo(db.pattern, {as: 'exclude'});
db.pattern.belongsTo(db.tourn);

db.tiebreak.belongsTo(db.tiebreak_set);

db.tiebreak_set.belongsTo(db.tourn);
db.tiebreak_set.hasMany(db.tiebreak);

db.webpage.belongsTo(db.person, {as: 'last_editor'});
db.webpage.belongsTo(db.webpage, {as: 'parent'});
db.webpage.belongsTo(db.tourn);

db.webpage.hasMany(db.change_log);
db.webpage.hasMany(db.file);
db.webpage.hasMany(db.webpage, {as: 'child', foreignKey: 'parent_id'} );

db.jpool.belongsTo(db.category);
db.jpool.belongsTo(db.site);
db.jpool.belongsToMany(db.judge, {through: 'jpool_judges'});
db.jpool.belongsToMany(db.round, {through: 'jpool_rounds'});
db.jpool.hasMany(db.jpool_setting);

db.rpool.belongsTo(db.tourn);
db.rpool.belongsToMany(db.room, {through: 'rpool_rooms'});
db.rpool.belongsToMany(db.round, {through: 'rpool_rounds'});
db.rpool.hasMany(db.rpool_setting);

// Settings

db.tourn_setting.belongsTo(db.tourn);
db.category_setting.belongsTo(db.category);
db.judge_setting.belongsTo(db.judge);
db.event_setting.belongsTo(db.event);
db.entry_setting.belongsTo(db.entry);
db.round_setting.belongsTo(db.round);
db.circuit_setting.belongsTo(db.circuit);
db.person_setting.belongsTo(db.person);
db.jpool_setting.belongsTo(db.jpool);
db.rpool_setting.belongsTo(db.rpool);
db.tiebreak_set_setting.belongsTo(db.tiebreak_set);
db.school_setting.belongsTo(db.school);

db.tourn_setting.belongsTo(db.setting);
db.category_setting.belongsTo(db.setting);
db.judge_setting.belongsTo(db.setting);
db.event_setting.belongsTo(db.setting);
db.entry_setting.belongsTo(db.setting);
db.round_setting.belongsTo(db.setting);
db.circuit_setting.belongsTo(db.setting);
db.person_setting.belongsTo(db.setting);
db.jpool_setting.belongsTo(db.setting);
db.rpool_setting.belongsTo(db.setting);
db.tiebreak_set_setting.belongsTo(db.setting);
db.school_setting.belongsTo(db.setting);

db.setting.hasMany(db.tourn_setting);
db.setting.hasMany(db.category_setting);
db.setting.hasMany(db.judge_setting);
db.setting.hasMany(db.event_setting);
db.setting.hasMany(db.entry_setting);
db.setting.hasMany(db.round_setting);
db.setting.hasMany(db.circuit_setting);
db.setting.hasMany(db.person_setting);
db.setting.hasMany(db.jpool_setting);
db.setting.hasMany(db.rpool_setting);
db.setting.hasMany(db.tiebreak_set_setting);
db.setting.hasMany(db.school_setting);

db.setting_label.belongsTo(db.setting);

// Registration data
db.school.hasMany(db.entry);
db.school.hasMany(db.rating);
db.school.hasMany(db.fine);
db.school.hasMany(db.strike);
db.school.hasMany(db.school_setting);
db.school.hasMany(db.change_log);
db.school.hasMany(db.file);

db.school.belongsTo(db.tourn);
db.school.belongsTo(db.chapter);
db.school.belongsTo(db.person, {as: "registered_by"});
db.school.belongsTo(db.person, {as: "onsite_by"});

db.school.belongsToMany(db.person, {through: 'school_contacts'});

db.fine.belongsTo(db.person, {as: 'levied_by'});
db.fine.belongsTo(db.person, {as: 'deleted_by'});
db.fine.belongsTo(db.tourn);
db.fine.belongsTo(db.school);
db.fine.belongsTo(db.region);
db.fine.belongsTo(db.judge);
db.fine.hasMany(db.change_log);

db.entry.hasMany(db.ballot);
db.entry.hasMany(db.qualifier);
db.entry.hasMany(db.rating);
db.entry.hasMany(db.strike);
db.entry.hasMany(db.change_log);

db.entry.belongsTo(db.event);
db.entry.belongsTo(db.school);
db.entry.belongsTo(db.tourn);
db.entry.belongsTo(db.person, {as: "registered_by"});
db.entry.belongsToMany(db.student, {through: 'entry_students'});

db.qualifier.belongsTo(db.entry);
db.qualifier.belongsTo(db.tourn);
db.qualifier.belongsTo(db.tourn, {as : 'qualifier_tourn'});

db.judge.hasMany(db.fine);
db.judge.hasMany(db.ballot);
db.judge.hasMany(db.rating);
db.judge.hasMany(db.strike);
db.judge.hasMany(db.judge_setting);
db.judge.hasMany(db.change_log);

db.judge.belongsTo(db.school);
db.judge.belongsTo(db.category);
db.judge.belongsTo(db.category, {as: 'alt_category'});
db.judge.belongsTo(db.category, {as: 'covers'});

db.judge.belongsTo(db.chapter_judge);
db.judge.belongsTo(db.person);
db.judge.belongsTo(db.person, {as: 'person_request'});

db.judge.belongsToMany(db.jpool, {through: 'jpool_judges'});

db.judge_hire.belongsTo(db.person, {as: 'requestor'});
db.judge_hire.belongsTo(db.judge);
db.judge_hire.belongsTo(db.tourn);
db.judge_hire.belongsTo(db.school);
db.judge_hire.belongsTo(db.category);

// Specialty registration data later

db.concession.belongsTo(db.tourn);
db.concession.hasMany(db.concession_purchase);

db.concession_purchase.belongsTo(db.concession);
db.concession_purchase.belongsTo(db.school);

db.email.belongsTo(db.person, {as: 'sender'});
db.email.belongsTo(db.tourn);
db.email.belongsTo(db.circuit);

db.file.belongsTo(db.tourn);
db.file.belongsTo(db.school);
db.file.belongsTo(db.event);
db.file.belongsTo(db.circuit);
db.file.belongsTo(db.webpage);

db.hotel.belongsTo(db.tourn);

db.housing_slots.belongsTo(db.tourn);

db.housing.belongsTo(db.person, {as: 'requestor'});
db.housing.belongsTo(db.tourn);
db.housing.belongsTo(db.student);
db.housing.belongsTo(db.judge);
db.housing.belongsTo(db.school);

db.stats.belongsTo(db.event);

// Pref Sheets
db.rating.belongsTo(db.tourn);
db.rating.belongsTo(db.school);
db.rating.belongsTo(db.entry);
db.rating.belongsTo(db.rating_tier);
db.rating.belongsTo(db.judge);
db.rating.belongsTo(db.rating_subset);

db.rating_tier.hasMany(db.rating);
db.rating_tier.belongsTo(db.category);
db.rating_tier.belongsTo(db.rating_subset);

db.rating_subset.hasMany(db.event);
db.rating_subset.hasMany(db.rating);
db.rating_subset.hasMany(db.rating_tier);
db.rating_subset.belongsTo(db.category);

db.strike.belongsTo(db.tourn);
db.strike.belongsTo(db.judge);
db.strike.belongsTo(db.event);
db.strike.belongsTo(db.entry);
db.strike.belongsTo(db.school);
db.strike.belongsTo(db.region);
db.strike.belongsTo(db.timeslot);
db.strike.belongsTo(db.person, {as: 'entered_by'});
db.strike.belongsTo(db.strike_timeslot);
db.strike.hasMany(db.change_log);

db.strike_timeslot.hasMany(db.strike);
db.strike_timeslot.belongsTo(db.category);

// Rounds & results

db.round.belongsTo(db.event);
db.round.belongsTo(db.timeslot);
db.round.belongsTo(db.site);
db.round.belongsTo(db.tiebreak_set);

db.round.hasMany(db.change_log);
db.round.hasMany(db.round_setting);
db.round.belongsToMany(db.rpool, {through: 'rpool_rounds'});

db.panel.belongsTo(db.room);
db.panel.belongsTo(db.round);
db.panel.hasMany(db.ballot);
db.panel.hasMany(db.change_log, {as : 'old_panel'});
db.panel.hasMany(db.change_log, {as : 'new_panel'});

db.ballot.belongsTo(db.entry);
db.ballot.belongsTo(db.judge);
db.ballot.belongsTo(db.panel); 
db.ballot.belongsTo(db.person, {as: 'collected_by'});
db.ballot.belongsTo(db.person, {as: 'entered_by'});
db.ballot.belongsTo(db.person, {as: 'audited_by'});
db.ballot.belongsTo(db.person, {as: 'hangout_admin'});

db.ballot.hasMany(db.score);

db.score.belongsTo(db.ballot);
db.score.belongsTo(db.student);

// Published Results

db.result.belongsTo(db.result_set);
db.result.belongsTo(db.entry);
db.result.belongsTo(db.student);
db.result.belongsTo(db.school);
db.result.belongsTo(db.round);

db.result_set.hasMany(db.result);
db.result_set.hasMany(db.file);
db.result_set.belongsTo(db.tourn);
db.result_set.belongsTo(db.event);

db.result_value.belongsTo(db.result);

// Person & identities

db.person.hasMany(db.login);
db.person.hasMany(db.student);
db.person.hasMany(db.judge);
db.person.hasMany(db.conflict);
db.person.hasMany(db.person_setting);
db.person.hasMany(db.change_log);

db.person.belongsToMany(db.tourn, {through: 'permission'});
db.person.belongsToMany(db.tourn, {through: 'tourn_ignore'});
db.person.belongsToMany(db.region, {through: 'permission'});
db.person.belongsToMany(db.chapter, {through: 'permission'});
db.person.belongsToMany(db.circuit, {through: 'permission'});

db.conflict.belongsTo(db.person);
db.conflict.belongsTo(db.person, {as: 'conflicted'});
db.conflict.belongsTo(db.chapter);
db.conflict.belongsTo(db.person, {as: 'added_by'});

db.login.belongsTo(db.person);

//Permissions
db.permission.belongsTo(db.person);
db.permission.belongsTo(db.tourn);
db.permission.belongsTo(db.region);
db.permission.belongsTo(db.chapter);
db.permission.belongsTo(db.circuit);
db.permission.belongsTo(db.category);

db.category.hasMany(db.permission);
db.person.hasMany(db.permission);
db.tourn.hasMany(db.permission);
db.region.hasMany(db.permission);
db.chapter.hasMany(db.permission);
db.circuit.hasMany(db.permission);

// Sweepstakes.  Their very own special world.  That I hate.
db.sweep_set.belongsToMany(db.sweep_set, {through: 'sweep_include', as: 'parents'});
db.sweep_set.belongsToMany(db.sweep_set, {through: 'sweep_include', as: 'children'});
db.sweep_set.belongsToMany(db.event, {through: 'sweep_events'});

db.sweep_set.belongsTo(db.tourn);
db.sweep_set.hasMany(db.sweep_rule);
db.sweep_rule.belongsTo(db.sweep_set);

// Live Updates functions

db.follower.belongsTo(db.person, {as: 'follower'});
db.follower.belongsTo(db.judge);
db.follower.belongsTo(db.entry);
db.follower.belongsTo(db.school);
db.follower.belongsTo(db.tourn);
db.follower.belongsTo(db.person);

// Sessions
db.session.belongsTo(db.person, {as: 'su'});
db.session.belongsTo(db.person);
db.session.belongsTo(db.tourn);
db.session.belongsTo(db.event);
db.session.belongsTo(db.category);

// Initialize the data objects.

db.sequelize = sequelize;
db.Sequelize = Sequelize;

module.exports = db;

//db.Ballot.belongsTo(db.Person, {as: "CollectedBy"});

