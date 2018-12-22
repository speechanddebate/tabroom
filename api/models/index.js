"use strict";

const fs        = require("fs");
const path      = require("path");
const Sequelize = require("sequelize");

const basename = path.basename(module.filename);
const env      = process.env.NODE_ENV || "development";
const config   = require(__dirname + '/../config/config.json')[env];

const sequelize = new Sequelize(
	config.db.database, 
	config.db.username, 
	config.db.password, 
	config.db.sequelizeOptions
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

// Relations, lots and lots of relations
db.log.belongsTo(db.person,  { as: "Person",     foreignKey : "person"});
db.log.belongsTo(db.tourn,   { as: "Tourn",      foreignKey : "tourn"});
db.log.belongsTo(db.event,   { as: "Event",      foreignKey : "event"});
db.log.belongsTo(db.school,  { as: "School",     foreignKey : "school"});
db.log.belongsTo(db.entry,   { as: "Entry",      foreignKey : "entry"});
db.log.belongsTo(db.judge,   { as: "Judge",      foreignKey : "judge"});
db.log.belongsTo(db.section, { as: "oldSection", foreignKey : "old_panel"});
db.log.belongsTo(db.section, { as: "newSection", foreignKey : "new_section"});
db.log.belongsTo(db.fine,    { as: "Fine",       foreignKey : "fine"});

db.circuit.belongsToMany(db.tourn, {through: 'tourn_circuits'});
db.circuit.belongsToMany(db.person, {through: 'permissions'});
db.circuit.hasMany(db.circuit_membership);
db.circuit.hasMany(db.file, {as: "File", foreignKey: "circuit"});
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
db.room_strike.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.room_strike.belongsTo(db.entry);
db.room_strike.belongsTo(db.judge);

db.chapter.hasMany(db.student);
db.chapter.hasMany(db.school);
db.chapter.hasMany(db.chapter_judge);

db.chapter.belongsToMany(db.person, {through: 'permissions'});
db.chapter.belongsTo(db.district);

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
db.district.hasMany(db.chapter);
db.district.hasMany(db.permission);

db.student.belongsTo(db.chapter);
db.student.belongsTo(db.person);
db.student.belongsTo(db.person, {as: "person_request"});
db.student.belongsToMany(db.entry, {through: 'entry_students'});
db.student.hasMany(db.score);
db.student.hasMany(db.follower);

db.region.belongsTo(db.circuit);
db.region.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.region.hasMany(db.strike);
db.region.hasMany(db.judge_hire);
db.region.hasMany(db.fine);
db.region.belongsToMany(db.person, {through: 'permissions'});

db.region.belongsToMany(db.chapter, {through: 'chapter_circuits'});
db.chapter.belongsToMany(db.region, {through: 'chapter_circuits'});

// Tournament wide relations
db.tourn.hasMany(db.log);
db.tourn.hasMany(db.category,     { as : "Categories",   foreignKey : "tourn"});
db.tourn.hasMany(db.concession,   { as : "Concession",   foreignKey : "tourn"});
db.tourn.hasMany(db.email,        { as : "Email",        foreignKey : "tourn"});
db.tourn.hasMany(db.event,        { as : "Events",       foreignKey : "tourn"});
db.tourn.hasMany(db.entry,        { as : "Entry",        foreignKey : "tourn"});
db.tourn.hasMany(db.file,         { as : "Files",        foreignKey : "tourn"});
db.tourn.hasMany(db.fine,         { as : "Fines",        foreignKey : "tourn"});
db.tourn.hasMany(db.follower,     { as : "Followers",    foreignKey : "tourn"});
db.tourn.hasMany(db.hotel,        { as : "Hotels",       foreignKey : "tourn"});
db.tourn.hasMany(db.region,       { as : "Regions",      foreignKey : "tourn"});
db.tourn.hasMany(db.result_set,   { as : "ResultSets",   foreignKey : "tourn"});
db.tourn.hasMany(db.school,       { as : "Schools",      foreignKey : "tourn"});
db.tourn.hasMany(db.timeslot,     { as : "Timeslots",    foreignKey : "tourn"});
db.tourn.hasMany(db.tourn_fee,    { as : "TournFees",    foreignKey : "tourn"});
db.tourn.hasMany(db.tiebreak_set, { as : "TiebreakSets", foreignKey : "tourn"});
db.tourn.hasMany(db.webpage,      { as : "Pages",        foreignKey : "tourn"});

db.tourn.belongsToMany(db.site,    { through: 'tourn_sites'});
db.tourn.belongsToMany(db.person,  { through: 'permissions'});
db.tourn.belongsToMany(db.circuit, { through: 'tourn_circuits'});
db.tourn.belongsToMany(db.person,  { through: 'tourn_ignore'});

db.tourn_fee.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.timeslot.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.timeslot.hasMany(db.round);
db.timeslot.hasMany(db.strike);

db.category.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.category.belongsTo(db.pattern, {as: "Pattern", foreignKey: "pattern"});
db.category.hasMany(db.event, {as: "Events", foreignKey: "category"});
db.category.hasMany(db.judge);
db.category.hasMany(db.log);

db.event.belongsTo(db.tourn,         { as: "Tourn",        foreignKey : "tourn"});
db.event.belongsTo(db.category,      { as: "Category",     foreignKey : "category"});
db.event.belongsTo(db.pattern,       { as: "Pattern",      foreignKey : "pattern"});
db.event.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey : "rating_subset"});

db.event.hasMany(db.file,       { as: "Files",      foreignKey : "event"});
db.event.hasMany(db.stats,      { as: "Stats",      foreignKey : "event"});
db.event.hasMany(db.entry,      { as: "Entries",    foreignKey : "event"});
db.event.hasMany(db.round,      { as: "Rounds",     foreignKey : "event"});
db.event.hasMany(db.log, { as: "Logs", foreignKey : "event"});

db.event.belongsToMany(db.sweep_set, { through: 'sweep_events'});

db.pattern.belongsTo(db.pattern, { as: 'exclude'});
db.pattern.belongsTo(db.tourn,   { as: "Tourn",      foreignKey : "tourn"});
db.pattern.hasMany(db.event,     { as: "Events",     foreignKey : "pattern"});
db.pattern.hasMany(db.category,  { as: "Categories", foreignKey : "pattern"});

db.tiebreak.belongsTo(db.tiebreak_set);
db.tiebreak.belongsTo(db.tiebreak_set, {as: "child"});

db.tiebreak_set.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.tiebreak_set.hasMany(db.tiebreak);

db.webpage.belongsTo(db.person, {as: 'Editor', foreignKey: "last_editor"});
db.webpage.belongsTo(db.webpage, {as: 'Parent', foreignKey: "tourn"});
db.webpage.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.webpage.hasMany(db.log,     { as: "Log",   foreignKey : "webpage"});
db.webpage.hasMany(db.file,    { as: "File",  foreignKey : "webpage"});
db.webpage.hasMany(db.webpage, { as: 'child', foreignKey : 'parent'} );

db.jpool.belongsTo(db.category, {as: "Category", foreignKey: "category"});
db.jpool.belongsTo(db.site);
db.jpool.belongsToMany(db.judge, {through: 'jpool_judges'});
db.jpool.belongsToMany(db.round, {through: 'jpool_rounds'});

db.rpool.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.rpool.belongsToMany(db.room, {through: 'rpool_rooms'});
db.rpool.belongsToMany(db.round, {through: 'rpool_rounds'});

// Settings

db.tourn_setting.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.tourn.hasMany(db.tourn_setting);
db.tourn_setting.belongsTo(db.setting);
db.setting.hasMany(db.tourn_setting);

db.category_setting.belongsTo(db.category, {as: "Category", foreignKey: "category"});
db.category.hasMany(db.category_setting);
db.category_setting.belongsTo(db.setting);
db.setting.hasMany(db.category_setting);

db.circuit_setting.belongsTo(db.circuit);
db.circuit.hasMany(db.circuit_setting);
db.circuit_setting.belongsTo(db.setting);
db.setting.hasMany(db.circuit_setting);

db.chapter_setting.belongsTo(db.chapter);
db.chapter.hasMany(db.chapter_setting);
db.chapter_setting.belongsTo(db.setting);
db.setting.hasMany(db.chapter_setting);

db.judge_setting.belongsTo(db.judge);
db.judge.hasMany(db.judge_setting);
db.judge_setting.belongsTo(db.setting);
db.setting.hasMany(db.judge_setting);

db.event_setting.belongsTo(db.event, {as: "Event", foreignKey: "event"});
db.event.hasMany(db.event_setting, {as: "EventSettings", foreignKey: "event"});
db.event_setting.belongsTo(db.setting, {as: "Setting", foreignKey: "setting"});
db.setting.hasMany(db.event_setting, {as: "EventSettings", foreignKey: "setting"});

db.entry_setting.belongsTo(db.entry);
db.entry.hasMany(db.entry_setting);
db.entry_setting.belongsTo(db.setting);
db.setting.hasMany(db.entry_setting);

db.round_setting.belongsTo(db.round);
db.round.hasMany(db.round_setting);
db.round_setting.belongsTo(db.setting);
db.setting.hasMany(db.round_setting);

db.person_setting.belongsTo(db.person);
db.person.hasMany(db.person_setting);
db.person_setting.belongsTo(db.setting);
db.setting.hasMany(db.person_setting);

db.jpool_setting.belongsTo(db.jpool);
db.jpool.hasMany(db.jpool_setting);
db.jpool_setting.belongsTo(db.setting);
db.setting.hasMany(db.jpool_setting);

db.rpool_setting.belongsTo(db.rpool);
db.rpool.hasMany(db.rpool_setting);
db.rpool_setting.belongsTo(db.setting);
db.setting.hasMany(db.rpool_setting);

db.tiebreak_set_setting.belongsTo(db.tiebreak_set);
db.tiebreak_set.hasMany(db.tiebreak_set_setting);
db.tiebreak_set_setting.belongsTo(db.setting);
db.setting.hasMany(db.tiebreak_set_setting);

db.school_setting.belongsTo(db.school);
db.school.hasMany(db.school_setting);
db.school_setting.belongsTo(db.setting);
db.setting.hasMany(db.school_setting);

// Registration data
db.school.hasMany(db.entry,      { as: "Entries",    foreignKey : "school"});
db.school.hasMany(db.rating,     { as: "Ratings",    foreignKey : "school"});
db.school.hasMany(db.fine,       { as: "Fines",      foreignKey : "school"});
db.school.hasMany(db.strike,     { as: "Strikes",    foreignKey : "school"});
db.school.hasMany(db.log,        { as: "Logs",       foreignKey : "school"});
db.school.hasMany(db.file,       { as: "Files",      foreignKey : "school"});
db.school.hasMany(db.judge_hire, { as: "JudgeHires", foreignKey : "school"});

db.school.belongsTo(db.tourn,   { as: "Tourn",   foreignKey : "tourn"});
db.school.belongsTo(db.chapter, { as: "Chapter", foreignKey : "chapter"});
db.school.belongsTo(db.person,  { as: "registered_by"});
db.school.belongsTo(db.person,  { as: "onsite_by"});

db.school.belongsToMany(db.person, {through: 'school_contacts'});

db.fine.belongsTo(db.person, { as: 'levied_by'});
db.fine.belongsTo(db.person, { as: 'deleted_by'});
db.fine.belongsTo(db.tourn,  { as: "Tourn",  foreignKey : "tourn"});
db.fine.belongsTo(db.school, { as: "School", foreignKey : "school"});
db.fine.belongsTo(db.region, { as: "Region", foreignKey : "region"});
db.fine.belongsTo(db.judge,  { as: "Judge",  foreignKey : "judge"});
db.fine.hasMany(db.log);

db.entry.hasMany(db.ballot);
db.entry.hasMany(db.qualifier);
db.entry.hasMany(db.rating);
db.entry.hasMany(db.strike);
db.entry.hasMany(db.log);

db.entry.belongsTo(db.tourn,       { as: "Tourn",        foreignKey : "tourn"});
db.entry.belongsTo(db.school,      { as: "School",       foreignKey : "school"});
db.entry.belongsTo(db.event,       { as: "Event",        foreignKey : "event"});
db.entry.belongsTo(db.person,      { as: "RegisteredBy", foreignKey : "registered_by"});
db.entry.belongsToMany(db.student, { through: 'entry_students'});

db.qualifier.belongsTo(db.entry);
db.qualifier.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.qualifier.belongsTo(db.tourn, {as : 'qualifier_tourn'});

db.judge.hasMany(db.fine);
db.judge.hasMany(db.ballot);
db.judge.hasMany(db.rating);
db.judge.hasMany(db.strike);
db.judge.hasMany(db.log);

db.judge.belongsTo(db.school);
db.judge.belongsTo(db.category, {as: "Category", foreignKey: "category"});
db.judge.belongsTo(db.category, {as: 'alt_category'});
db.judge.belongsTo(db.category, {as: 'covers'});

db.judge.belongsTo(db.chapter_judge);
db.judge.belongsTo(db.person);
db.judge.belongsTo(db.person, {as: 'person_request'});

db.judge.belongsToMany(db.jpool, {through: 'jpool_judges'});

db.judge_hire.belongsTo(db.person,   { as: 'Requestor', foreignKey : "requestor"});
db.judge_hire.belongsTo(db.judge,    { as: "Judge",     foreignKey : "judge"});
db.judge_hire.belongsTo(db.tourn,    { as: "Tourn",     foreignKey : "tourn"});
db.judge_hire.belongsTo(db.school,   { as: "School",    foreignKey : "school"});
db.judge_hire.belongsTo(db.region,   { as: "Region",    foreignKey : "region"});
db.judge_hire.belongsTo(db.category, { as: "Category",  foreignKey : "category"});

// Specialty registration data later

db.concession.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.concession.hasMany(db.concession_purchase);
db.concession.hasMany(db.concession_type);

db.concession_option.belongsTo(db.concession_type);

db.concession_option.belongsToMany(
	db.concession_purchase, 
	{through: 'concession_purchase_option'}
);

db.concession_purchase.belongsToMany(
	db.concession_option, 
	{through: 'concession_purchase_option'}
);

db.concession_purchase.belongsTo(db.concession);
db.concession_purchase.belongsTo(db.school);

db.concession_type.hasMany(db.concession_option);
db.concession_type.belongsTo(db.concession);

db.email.belongsTo(db.person, {as: 'sender'});
db.email.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.email.belongsTo(db.circuit);

db.file.belongsTo(db.tourn,   { as: "Tourn",   foreignKey : "tourn"});
db.file.belongsTo(db.school,  { as: "School",  foreignKey : "school"});
db.file.belongsTo(db.event,   { as: "Event",   foreignKey : "event"});
db.file.belongsTo(db.circuit, { as: "Circuit", foreignKey : "circuit"});
db.file.belongsTo(db.webpage, { as: "Webpage", foreignKey : "webpage"});

db.hotel.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.housing_slots.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.housing.belongsTo(db.person, {as: 'requestor'});
db.housing.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.housing.belongsTo(db.student);
db.housing.belongsTo(db.judge);
db.housing.belongsTo(db.school);

db.stats.belongsTo(db.event, {as: "Event", foreignKey: "event"});

// Pref Sheets
db.rating.belongsTo(db.tourn,         { as: "Tourn",        foreignKey : "tourn"});
db.rating.belongsTo(db.school,        { as: "School",       foreignKey : "school"});
db.rating.belongsTo(db.entry,         { as: "Entry",        foreignKey : "entry"});
db.rating.belongsTo(db.rating_tier,   { as: "RatingTier",   foreignKey : "rating_tier"});
db.rating.belongsTo(db.judge,         { as: "Judge",        foreignKey : "judge"});
db.rating.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey : "rating_subset"});

db.rating_tier.hasMany(db.rating,          { as: "Ratings",      foreignKey : "rating_tier"});
db.rating_tier.belongsTo(db.category,      { as: "Category",     foreignKey : "category"});
db.rating_tier.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey : "rating_subset"});

db.rating_subset.hasMany(db.event,       { as: "Events",      foreignKey : "rating_subset"});
db.rating_subset.hasMany(db.rating,      { as: "Ratings",     foreignKey : "rating_subset"});
db.rating_subset.hasMany(db.rating_tier, { as: "RatingTiers", foreignKey : "rating_subset"});
db.rating_subset.belongsTo(db.category,  { as: "Category",    foreignKey : "category"});

db.strike.belongsTo(db.tourn,    { as: "Tourn",     foreignKey : "tourn"});
db.strike.belongsTo(db.judge,    { as: "Judge",     foreignKey : "judge"});
db.strike.belongsTo(db.event,    { as: "Event",     foreignKey : "event"});
db.strike.belongsTo(db.entry,    { as: "Entry",     foreignKey : "entry"});
db.strike.belongsTo(db.school,   { as: "School",    foreignKey : "school"});
db.strike.belongsTo(db.region,   { as: "Region",    foreignKey : "region"});
db.strike.belongsTo(db.timeslot, { as: "Timeslot",  foreignKey : "timeslot"});
db.strike.belongsTo(db.person,   { as: "EnteredBy", foreignKey : 'entered_by'});
db.strike.belongsTo(db.shift,    { as: "Shift",     foreignKey : "shift"});

db.strike.hasMany(db.log,        { as: "Log", foreignKey: "event"});

db.shift.hasMany(db.strike);
db.shift.belongsTo(db.category, {as: "Category", foreignKey: "category"});

// Rounds & results
db.round.belongsTo(db.event,        { as: "Event",       foreignKey : "event"});
db.round.belongsTo(db.timeslot,     { as: "Timeslot",    foreignKey : "timeslot"});
db.round.belongsTo(db.site,         { as: "Site",        foreignKey : "site"});
db.round.belongsTo(db.tiebreak_set, { as: "TiebreakSet", foreignKey : "tiebreak_set"});

db.round.hasMany(db.log);

db.round.belongsToMany(
	db.rpool, 
	{through: 'rpool_rounds'}
);

db.section.belongsTo(db.room,   { as : "Room",    foreignKey : "room"});
db.section.belongsTo(db.round,  { as : "Round",   foreignKey : "round"});
db.section.belongsTo(db.ballot, { as : "Ballot",  foreignKey : "ballot"});
db.section.hasMany(db.log,      { as : 'LogsOld', foreignKey : "old_panel"});
db.section.hasMany(db.log,      { as : 'LogsNew', foreignKey : "new_panel"});

db.ballot.belongsTo(db.entry,   { as: "Entry",   foreignKey : "entry"});
db.ballot.belongsTo(db.judge,   { as: "Judge",   foreignKey : "judge"});
db.ballot.belongsTo(db.section, { as: "Section", foreignKey : "panel"});
db.ballot.belongsTo(db.person,  { as: 'collected_by'});
db.ballot.belongsTo(db.person,  { as: 'entered_by'});
db.ballot.belongsTo(db.person,  { as: 'audited_by'});

db.ballot.hasMany(db.score,    { as: "Scores",  foreignKey : "ballot"});
db.score.belongsTo(db.ballot,  { as: "Ballot",  foreignKey : "ballot"});
db.score.belongsTo(db.student, { as: "Student", foreignKey : "student"});

// Published Results

db.result.belongsTo(db.result_set,   { as: "ResultSet",    foreignKey : "result_set"});
db.result.belongsTo(db.entry,        { as: "Entry",        foreignKey : "entry"});
db.result.belongsTo(db.student,      { as: "Student",      foreignKey : "student"});
db.result.belongsTo(db.school,       { as: "School",       foreignKey : "school"});
db.result.belongsTo(db.round,        { as: "Round",        foreignKey : "round"});

db.result.hasMany(db.result_value,   { as: "ResultValues", foreignKey : "result"});

db.result_set.hasMany(db.result,     { as: "Result", foreignKey : "result_set"});
db.result_set.belongsTo(db.tourn,    { as: "Tourn",  foreignKey : "tourn"});
db.result_set.belongsTo(db.event,    { as: "Event",  foreignKey : "event"});
db.result_value.belongsTo(db.result, { as: "Result", foreignKey : "school"});

// Person & identities

db.person.hasMany(db.login);
db.person.hasMany(db.student);
db.person.hasMany(db.judge);
db.person.hasMany(db.conflict);
db.person.hasMany(db.log);

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
db.permission.belongsTo(db.district);
db.permission.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.permission.belongsTo(db.region);
db.permission.belongsTo(db.chapter);
db.permission.belongsTo(db.circuit);
db.permission.belongsTo(db.category, {as: "Category", foreignKey: "category"});

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

db.sweep_set.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.sweep_set.hasMany(db.sweep_rule);
db.sweep_rule.belongsTo(db.sweep_set);

// Live Updates functions

db.follower.belongsTo(db.person, {as: 'follower'});
db.follower.belongsTo(db.judge);
db.follower.belongsTo(db.entry);
db.follower.belongsTo(db.school);
db.follower.belongsTo(db.student);
db.follower.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.follower.belongsTo(db.person);

// Sessions
db.session.belongsTo(db.person, {as: 'Su', foreignKey: 'su'});
db.session.belongsTo(db.person, {as: 'Person', foreignKey: 'person'});
db.session.belongsTo(db.tourn, {as: 'Tourn', foreignKey: 'tourn'});
db.session.belongsTo(db.event, {as: 'Event', foreignKey: 'event'});
db.session.belongsTo(db.category, {as: 'Category', foreignKey: 'category'});

// Initialize the data objects.

db.sequelize = sequelize;
db.Sequelize = Sequelize;

module.exports = db;

//db.Ballot.belongsTo(db.Person, {as: "CollectedBy"});

