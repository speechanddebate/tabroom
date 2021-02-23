"use strict";

import config from '../../config/config';
import fs from 'fs';
import path from 'path';
import Sequelize from 'sequelize';

const basename = path.basename(module.filename);

const sequelize = new Sequelize(
	config.DB_DATABASE,
	config.DB_USER,
	config.DB_PASS,
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
		const model = require(path.join(__dirname, file))(sequelize, Sequelize.DataTypes)
		db[model.name] = model;
	});

Object.keys(db).forEach(function(modelName) {
	if ("associate" in db[modelName]) {
		db[modelName].associate(db);
	}
});

// Relations, lots and lots of relations
db.changeLog.belongsTo(db.person,  { as: "Person",     foreignKey: "person"});
db.changeLog.belongsTo(db.tourn,   { as: "Tourn",      foreignKey: "tourn"});
db.changeLog.belongsTo(db.event,   { as: "Event",      foreignKey: "event"});
db.changeLog.belongsTo(db.school,  { as: "School",     foreignKey: "school"});
db.changeLog.belongsTo(db.entry,   { as: "Entry",      foreignKey: "entry"});
db.changeLog.belongsTo(db.judge,   { as: "Judge",      foreignKey: "judge"});
db.changeLog.belongsTo(db.section, { as: "oldSection", foreignKey: "old_panel"});
db.changeLog.belongsTo(db.section, { as: "newSection", foreignKey: "new_section"});
db.changeLog.belongsTo(db.fine,    { as: "Fine",       foreignKey: "fine"});

db.circuit.belongsToMany(db.tourn,   { as: "Tourns",   foreignKey: "circuit", through: 'tourn_circuit'});
db.circuit.belongsToMany(db.person,  { as: "Persons",  foreignKey: "circuit", through: 'permission'});
db.circuit.belongsToMany(db.chapter, { as: "Chapters", foreignKey: "circuit", through: 'chapter_circuit'});

db.circuit.hasMany(db.file,               { as: "Files",              foreignKey: "circuit"});
db.circuit.hasMany(db.permission,         { as: "Permissions",        foreignKey: "circuit"});

db.site.belongsTo(db.person,  { as: 'Host',    foreignKey: "host"});
db.site.belongsTo(db.circuit, { as: "Circuit", foreignKey: "circuit"});

db.site.hasMany(db.room,  { as: "Rooms",  foreignKey: "site"});
db.site.hasMany(db.round, { as: "Rounds", foreignKey: "site"});

db.site.belongsToMany(db.tourn, {through: 'tourn_sites'});

db.room.hasMany(db.room_strike, {as: "Strikes", foreignKey: "room"});

db.room.belongsTo(db.site, {as: "Site", foreignKey: "site"});

db.room.belongsToMany(db.rpool, {as: "RPools", foreignKey: "room", through: 'pool_rooms'});
db.room.belongsToMany(db.round, {as: "Rounds", foreignKey: "room", through: 'panel'});

db.room_strike.belongsTo(db.room,  { as: "Room",  foreignKey: "room"});
db.room_strike.belongsTo(db.event, { as: "Event", foreignKey: "event"});
db.room_strike.belongsTo(db.tourn, { as: "Tourn", foreignKey: "tourn"});
db.room_strike.belongsTo(db.entry, { as: "Entry", foreignKey: "entry"});
db.room_strike.belongsTo(db.judge, { as: "Judge", foreignKey: "judge"});

db.chapter.hasMany(db.student,       { as: "Students",      foreignKey: "chapter"});
db.chapter.hasMany(db.permission,    { as: "Permissions",   foreignKey: "chapter"});
db.chapter.hasMany(db.school,        { as: "Schools",       foreignKey: "chapter"});
db.chapter.hasMany(db.chapterJudge, { as: "ChapterJudges", foreignKey: "chapter"});

db.chapter.belongsTo(db.district, {as: "District", foreignKey: "district"});

db.chapter.belongsToMany(db.region,  { as: "Regions",  foreignKey: "circuit", through: 'chapter_circuit'});
db.chapter.belongsToMany(db.circuit, { as: "Circuits", foreignKey: "circuit", through: 'chapter_circuit'});
db.chapter.belongsToMany(db.person,  { as: "Persons",  foreignKey: "circuit", through: 'permission'});


db.chapterCircuit.belongsTo(db.circuit, {as: "Circuit", foreignKey: "circuit"});
db.chapterCircuit.belongsTo(db.chapter, {as: "Chapter", foreignKey: "chapter"});
db.chapterCircuit.belongsTo(db.region,  {as: "Region", foreignKey: "region"});

db.chapterJudge.hasMany(db.judge,     { as: "Judges",        foreignKey: "chapter_judge"});

db.chapterJudge.belongsTo(db.chapter, { as: "Chapter",       foreignKey: "chapter"});
db.chapterJudge.belongsTo(db.person,  { as: "Person",        foreignKey: "person"});
db.chapterJudge.belongsTo(db.person,  { as: "PersonRequest", foreignKey: "person_request"});


db.district.hasMany(db.chapter,    { as: "Chapters",    foreignKey: "district"});
db.district.hasMany(db.permission, { as: "Permissions", foreignKey: "district"});


db.student.belongsTo(db.chapter, { as: "Chapter",       foreignKey: "chapter"});
db.student.belongsTo(db.person,  { as: "Person",        foreignKey: "person"});
db.student.belongsTo(db.person,  { as: "PersonRequest", foreignKey: "person_request"});

db.student.hasMany(db.score,       { as: "Scores",    foreignKey: "student"});
db.student.hasMany(db.follower,    { as: "Followers", foreignKey: "student"});

db.student.belongsToMany(db.entry, { as: "Entries",   foreignKey: "student", through: 'entry_students'});


db.region.belongsTo(db.circuit, { as: "Circuit", foreignKey: "circuit"});
db.region.belongsTo(db.tourn,   { as: "Tourn", foreignKey: "tourn"});

db.region.hasMany(db.strike,     { as: "Strikes",     foreignKey: "region"});
db.region.hasMany(db.permission, { as: "Permissions", foreignKey: "region"});
db.region.hasMany(db.judge_hire, { as: "JudgeHires",  foreignKey: "region"});
db.region.hasMany(db.fine,       { as: "Fines",       foreignKey: "region"});

db.region.belongsToMany(db.person,  { as: "Persons",  foreignKey: "region", through: 'permission'});
db.region.belongsToMany(db.chapter, { as: "Chapters", foreignKey: "region", through: 'chapter_circuit'});

// Tournament wide relations
db.tourn.hasMany(db.changeLog,          { as: "ChangeLogs",         foreignKey: "tourn"});
db.tourn.hasMany(db.category,     { as: "Categories",   foreignKey: "tourn"});
db.tourn.hasMany(db.concession,   { as: "Concession",   foreignKey: "tourn"});
db.tourn.hasMany(db.email,        { as: "Email",        foreignKey: "tourn"});
db.tourn.hasMany(db.event,        { as: "Events",       foreignKey: "tourn"});
db.tourn.hasMany(db.entry,        { as: "Entry",        foreignKey: "tourn"});
db.tourn.hasMany(db.file,         { as: "Files",        foreignKey: "tourn"});
db.tourn.hasMany(db.fine,         { as: "Fines",        foreignKey: "tourn"});
db.tourn.hasMany(db.follower,     { as: "Followers",    foreignKey: "tourn"});
db.tourn.hasMany(db.hotel,        { as: "Hotels",       foreignKey: "tourn"});
db.tourn.hasMany(db.region,       { as: "Regions",      foreignKey: "tourn"});
db.tourn.hasMany(db.result_set,   { as: "ResultSets",   foreignKey: "tourn"});
db.tourn.hasMany(db.school,       { as: "Schools",      foreignKey: "tourn"});
db.tourn.hasMany(db.timeslot,     { as: "Timeslots",    foreignKey: "tourn"});
db.tourn.hasMany(db.tourn_fee,    { as: "TournFees",    foreignKey: "tourn"});
db.tourn.hasMany(db.tiebreak_set, { as: "TiebreakSets", foreignKey: "tourn"});
db.tourn.hasMany(db.webpage,      { as: "Pages",        foreignKey: "tourn"});
db.tourn.hasMany(db.permission,   { as: "Permissions",  foreignKey: "tourn"});

db.tourn.belongsToMany(db.site,    { as: "Sites",    foreignKey: "tourn", through: 'tourn_sites'});
db.tourn.belongsToMany(db.person,  { as: "Persons",  foreignKey: "tourn", through: 'permissions'});
db.tourn.belongsToMany(db.circuit, { as: "Circuits", foreignKey: "tourn", through: 'tourn_circuit'});

db.tourn_fee.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.timeslot.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.timeslot.hasMany(db.round,  { as: "Rounds",  foreignKey: "timeslot"});
db.timeslot.hasMany(db.strike, { as: "Strikes", foreignKey: "timeslot"});

db.category.belongsTo(db.tourn,    { as: "Tourn",   foreignKey: "tourn"});
db.category.belongsTo(db.pattern,  { as: "Pattern", foreignKey: "pattern"});

db.category.hasMany(db.event,      { as: "Events",      foreignKey: "category"});
db.category.hasMany(db.judge,      { as: "Judges",      foreignKey: "category"});
db.category.hasMany(db.changeLog,        { as: "ChangeLogs",        foreignKey: "category"});
db.category.hasMany(db.permission, { as: "Permissions", foreignKey: "category"});


db.event.belongsTo(db.tourn,         { as: "Tourn",        foreignKey: "tourn"});
db.event.belongsTo(db.category,      { as: "Category",     foreignKey: "category"});
db.event.belongsTo(db.pattern,       { as: "Pattern",      foreignKey: "pattern"});
db.event.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey: "rating_subset"});

db.event.hasMany(db.file,  { as: "Files",   foreignKey: "event"});
db.event.hasMany(db.stats, { as: "Stats",   foreignKey: "event"});
db.event.hasMany(db.entry, { as: "Entries", foreignKey: "event"});
db.event.hasMany(db.round, { as: "Rounds",  foreignKey: "event"});
db.event.hasMany(db.changeLog,   { as: "ChangeLogs",    foreignKey: "event"});

db.event.belongsToMany(db.sweep_set, { through: 'sweep_events'});


db.pattern.hasMany(db.event,     { as: "Events",     foreignKey: "pattern"});
db.pattern.hasMany(db.category,  { as: "Categories", foreignKey: "pattern"});

db.pattern.belongsTo(db.pattern, { as: "Exclude",    foreignKey: "exclude"});
db.pattern.belongsTo(db.tourn,   { as: "Tourn",      foreignKey: "tourn"});


db.tiebreak.belongsTo(db.tiebreak_set, {as: "TiebreakSet", foreignKey: "tiebreak_set"});


db.tiebreak_set.belongsTo(db.tourn,        { as: "Tourn", foreignKey: "tourn"});
db.tiebreak_set.belongsTo(db.tiebreak_set, { as: "Child", foreignKey: "child"});

db.tiebreak_set.hasMany(db.tiebreak, {as: "Tiebreaks", foreignKey: "tiebreak_set"});

db.webpage.belongsTo(db.person,  { as: 'Editor', foreignKey: "last_editor"});
db.webpage.belongsTo(db.webpage, { as: 'Parent', foreignKey: "tourn"});
db.webpage.belongsTo(db.tourn,   { as: "Tourn",  foreignKey: "tourn"});

db.webpage.hasMany(db.changeLog,       { as: "Log",    foreignKey: "webpage"});
db.webpage.hasMany(db.file,      { as: "File",   foreignKey: "webpage"});
db.webpage.hasMany(db.webpage,   { as: 'child',  foreignKey: 'parent'} );


db.jpool.belongsTo(db.category,  { as: "Category", foreignKey: "category"});
db.jpool.belongsTo(db.site,      { as: "Site", foreignKey: "site"});

db.jpool.belongsToMany(db.judge, { through: 'jpool_judges'});
db.jpool.belongsToMany(db.round, { through: 'jpool_rounds'});


db.rpool.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.rpool.belongsToMany(db.room,  { as: "Rooms",  foreignKey: "rpool", through: 'rpool_rooms'});
db.rpool.belongsToMany(db.round, { as: "Rounds", foreignKey: "rpool", through: 'rpool_rounds'});

// Settings
db.tourn_setting.belongsTo(db.tourn,   { as: "Tourn",    foreignKey: "tourn"});
db.tourn.hasMany(db.tourn_setting,     { as: "Settings", foreignKey: "tourn"});
db.tourn_setting.belongsTo(db.setting, { as: "Setting",  foreignKey: "setting"});

db.categorySetting.belongsTo(db.category, { as: "Category", foreignKey: "category"});
db.category.hasMany(db.categorySetting,   { as: "Settings", foreignKey: "category"});
db.categorySetting.belongsTo(db.setting,  { as: "Setting",  foreignKey: "setting"});

db.circuitSetting.belongsTo(db.circuit, { as: "Circuit",  foreignKey: "circuit"});
db.circuit.hasMany(db.circuitSetting,   { as: "Settings", foreignKey: "circuit"});
db.circuitSetting.belongsTo(db.setting, { as: "Setting",  foreignKey: "setting"});

db.chapterSetting.belongsTo(db.chapter, { as: "Chapter",  foreignKey: "chapter"});
db.chapter.hasMany(db.chapterSetting,   { as: "Settings", foreignKey: "chapter"});
db.chapterSetting.belongsTo(db.setting, { as: "Setting",  foreignKey: "setting"});

db.judge_setting.belongsTo(db.judge,   { as: "Judge",    foreignKey: "judge"});
db.judge.hasMany(db.judge_setting,     { as: "Settings", foreignKey: "judge"});
db.judge_setting.belongsTo(db.setting, { as: "Setting",  foreignKey: "setting"});

db.eventSetting.belongsTo(db.event,   { as: "Event",    foreignKey: "event"});
db.event.hasMany(db.eventSetting,     { as: "Settings", foreignKey: "event"});
db.eventSetting.belongsTo(db.setting, { as: "Setting",  foreignKey: "setting"});

db.entrySetting.belongsTo(db.entry,   { as: "Entry", foreignKey: "entry"});
db.entry.hasMany(db.entrySetting,     { as: "Settings", foreignKey: "entry"});
db.entrySetting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});

db.round_setting.belongsTo(db.round,   { as: "Round", foreignKey: "round"});
db.round.hasMany(db.round_setting,     { as: "Settings", foreignKey: "round"});
db.round_setting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});

db.person_setting.belongsTo(db.person,  { as: "Person", foreignKey: "person"});
db.person.hasMany(db.person_setting,    { as: "Settings", foreignKey: "person"});
db.person_setting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});

db.jpool_setting.belongsTo(db.jpool);
db.jpool.hasMany(db.jpool_setting,     { as: "Settings", foreignKey: "jpool"});
db.jpool_setting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});

db.rpool_setting.belongsTo(db.rpool);
db.rpool.hasMany(db.rpool_setting,     { as: "Settings", foreignKey: "rpool"});
db.rpool_setting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});

db.tiebreak_set_setting.belongsTo(db.tiebreak_set);
db.tiebreak_set.hasMany(db.tiebreak_set_setting, { as: "Settings", foreignKey: "tiebreak_set"});
db.tiebreak_set_setting.belongsTo(db.setting,    { as: "Setting", foreignKey: "setting"});

db.school_setting.belongsTo(db.school,  { as: "School", foreignKey: "school"});
db.school_setting.belongsTo(db.setting, { as: "Setting", foreignKey: "setting"});
db.school.hasMany(db.school_setting, { as: "Settings", foreignKey: "school"});

// Registration data
db.school.hasMany(db.entry,          { as: "Entries",    foreignKey: "school"});
db.school.hasMany(db.rating,         { as: "Ratings",    foreignKey: "school"});
db.school.hasMany(db.fine,           { as: "Fines",      foreignKey: "school"});
db.school.hasMany(db.strike,         { as: "Strikes",    foreignKey: "school"});
db.school.hasMany(db.changeLog,            { as: "ChangeLogs",       foreignKey: "school"});
db.school.hasMany(db.file,           { as: "Files",      foreignKey: "school"});
db.school.hasMany(db.judge_hire,     { as: "JudgeHires", foreignKey: "school"});

db.school.belongsTo(db.tourn,   { as: "Tourn",        foreignKey: "tourn"});
db.school.belongsTo(db.chapter, { as: "Chapter",      foreignKey: "chapter"});
db.school.belongsTo(db.person,  { as: "RegisteredBy", foreignKey: "registered_by"});
db.school.belongsTo(db.person,  { as: "OnsiteBy" ,    foreignKey: "onsite_by"});

db.school.belongsToMany(db.person, {as: "Persons", foreignKey: "school", through: 'school_contacts'});

db.fine.belongsTo(db.person, { as: 'levied_by'});
db.fine.belongsTo(db.person, { as: 'deleted_by'});
db.fine.belongsTo(db.tourn,  { as: "Tourn",  foreignKey: "tourn"});
db.fine.belongsTo(db.school, { as: "School", foreignKey: "school"});
db.fine.belongsTo(db.region, { as: "Region", foreignKey: "region"});
db.fine.belongsTo(db.judge,  { as: "Judge",  foreignKey: "judge"});

db.fine.hasMany(db.changeLog,      { as: "ChangeLogs",   foreignKey: "fine"});


db.entry.hasMany(db.ballot,    { as: "Ballots",    foreignKey: "entry"});
db.entry.hasMany(db.qualifier, { as: "Qualifiers", foreignKey: "entry"});
db.entry.hasMany(db.rating,    { as: "Ratings",    foreignKey: "entry"});
db.entry.hasMany(db.strike,    { as: "Strikes",    foreignKey: "entry"});
db.entry.hasMany(db.changeLog,       { as: "ChangeLogs",       foreignKey: "entry"});

db.entry.belongsTo(db.tourn,   { as: "Tourn",        foreignKey: "tourn"});
db.entry.belongsTo(db.school,  { as: "School",       foreignKey: "school"});
db.entry.belongsTo(db.event,   { as: "Event",        foreignKey: "event"});
db.entry.belongsTo(db.person,  { as: "RegisteredBy", foreignKey: "registered_by"});

db.entry.belongsToMany(db.student, { as: "Students", foreignKey: "entry", through: 'entry_students'});
db.entry.belongsToMany(db.section, { as: "Sections", foreignKey: "entry", through: 'ballot'});


db.qualifier.belongsTo(db.entry, {as: "Entry", foreignKey: "entry"});
db.qualifier.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});
db.qualifier.belongsTo(db.tourn, {as: 'qualifier_tourn'});

db.judge.hasMany(db.fine,          { as: "Fines",    foreignKey: "judge"});
db.judge.hasMany(db.ballot,        { as: "Ballots",  foreignKey: "judge"});
db.judge.hasMany(db.rating,        { as: "Ratings",  foreignKey: "judge"});
db.judge.hasMany(db.strike,        { as: "Strikes",  foreignKey: "judge"});
db.judge.hasMany(db.changeLog,           { as: "ChangeLogs",     foreignKey: "judge"});

db.judge.belongsTo(db.school,        { as: "School",        foreignKey: "school"});
db.judge.belongsTo(db.category,      { as: "Category",      foreignKey: "category"});
db.judge.belongsTo(db.category,      { as: "AltCategory",   foreignKey: "alt_category"});
db.judge.belongsTo(db.category,      { as: "Covers",        foreignKey: "covers"});
db.judge.belongsTo(db.chapterJudge, { as: "ChapterJudge",  foreignKey: "chapter_judge"});
db.judge.belongsTo(db.person,        { as: "Person",        foreignKey: "person"});
db.judge.belongsTo(db.person,        { as: 'PersonRequest', foreignKey: "person_request"});

db.judge.belongsToMany(db.section, { as: "Sections", foreignKey: "judge", through: 'ballot'});
db.judge.belongsToMany(db.jpool,   { as: "JPools",  foreignKey: "judge", through: 'jpool_judges'});

db.judge_hire.belongsTo(db.person,   { as: 'Requestor', foreignKey: "requestor"});
db.judge_hire.belongsTo(db.judge,    { as: "Judge",     foreignKey: "judge"});
db.judge_hire.belongsTo(db.tourn,    { as: "Tourn",     foreignKey: "tourn"});
db.judge_hire.belongsTo(db.school,   { as: "School",    foreignKey: "school"});
db.judge_hire.belongsTo(db.region,   { as: "Region",    foreignKey: "region"});
db.judge_hire.belongsTo(db.category, { as: "Category",  foreignKey: "category"});

// Specialty registration data later

db.concession.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.concession.hasMany(db.concessionPurchase, { as: "Purchases", foreignKey: "concession"});
db.concession.hasMany(db.concessionType,     { as: "Types",     foreignKey: "concession"});

db.concessionType.hasMany(db.concessionOption, { as: "Options",    foreignKey: "concession_type"});
db.concessionType.belongsTo(db.concession,      { as: "Concession", foreignKey: "concession"});

db.concessionPurchase.belongsTo(db.concession, { as: "Concession", foreignKey: "concession"});
db.concessionPurchase.belongsTo(db.school,     { as: "School",     foreignKey: "school"});

db.concessionPurchase.belongsToMany( db.concessionOption,
	{ as: "Options", foreignKey: "concession_option", through: 'concession_purchase_option'}
);

db.concessionOption.belongsTo(db.concessionType, {as: "Type", foreignKey:"concession_type"});

db.concessionOption.belongsToMany( db.concessionPurchase,
	{ as: "Purchases", foreignKey: "concession_purchase", through: 'concession_purchase_option'}
);


db.email.belongsTo(db.person,  { as: 'Sender',  foreignKey: "sender"});
db.email.belongsTo(db.tourn,   { as: "Tourn",   foreignKey: "tourn"});
db.email.belongsTo(db.circuit, { as: "Circuit", foreignKey: "circuit"});


db.file.belongsTo(db.tourn,   { as: "Tourn",   foreignKey: "tourn"});
db.file.belongsTo(db.school,  { as: "School",  foreignKey: "school"});
db.file.belongsTo(db.event,   { as: "Event",   foreignKey: "event"});
db.file.belongsTo(db.circuit, { as: "Circuit", foreignKey: "circuit"});
db.file.belongsTo(db.webpage, { as: "Webpage", foreignKey: "webpage"});

db.hotel.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});


db.housing_slots.belongsTo(db.tourn, {as: "Tourn", foreignKey: "tourn"});

db.housing.belongsTo(db.person,  { as: "Requestor", foreignKey: "requestor"});
db.housing.belongsTo(db.tourn,   { as: "Tourn",     foreignKey: "tourn"});
db.housing.belongsTo(db.student, { as: "Student",   foreignKey: "student"});
db.housing.belongsTo(db.judge,   { as: "Judge",     foreignKey: "judge"});
db.housing.belongsTo(db.school,  { as: "School",    foreignKey: "school"});


db.stats.belongsTo(db.event, {as: "Event", foreignKey: "event"});

// Pref Sheets
db.rating.belongsTo(db.tourn,         { as: "Tourn",        foreignKey: "tourn"});
db.rating.belongsTo(db.school,        { as: "School",       foreignKey: "school"});
db.rating.belongsTo(db.entry,         { as: "Entry",        foreignKey: "entry"});
db.rating.belongsTo(db.rating_tier,   { as: "RatingTier",   foreignKey: "rating_tier"});
db.rating.belongsTo(db.judge,         { as: "Judge",        foreignKey: "judge"});
db.rating.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey: "rating_subset"});

db.rating_tier.hasMany(db.rating,          { as: "Ratings",      foreignKey: "rating_tier"});
db.rating_tier.belongsTo(db.category,      { as: "Category",     foreignKey: "category"});
db.rating_tier.belongsTo(db.rating_subset, { as: "RatingSubset", foreignKey: "rating_subset"});
db.rating_subset.hasMany(db.event,         { as: "Events",       foreignKey: "rating_subset"});
db.rating_subset.hasMany(db.rating,        { as: "Ratings",      foreignKey: "rating_subset"});
db.rating_subset.hasMany(db.rating_tier,   { as: "RatingTiers",  foreignKey: "rating_subset"});
db.rating_subset.belongsTo(db.category,    { as: "Category",     foreignKey: "category"});

db.strike.belongsTo(db.tourn,    { as: "Tourn",     foreignKey: "tourn"});
db.strike.belongsTo(db.judge,    { as: "Judge",     foreignKey: "judge"});
db.strike.belongsTo(db.event,    { as: "Event",     foreignKey: "event"});
db.strike.belongsTo(db.entry,    { as: "Entry",     foreignKey: "entry"});
db.strike.belongsTo(db.school,   { as: "School",    foreignKey: "school"});
db.strike.belongsTo(db.region,   { as: "Region",    foreignKey: "region"});
db.strike.belongsTo(db.timeslot, { as: "Timeslot",  foreignKey: "timeslot"});
db.strike.belongsTo(db.person,   { as: "EnteredBy", foreignKey: 'entered_by'});
db.strike.belongsTo(db.shift,    { as: "Shift",     foreignKey: "shift"});
db.strike.hasMany(db.changeLog,        { as: "Log",       foreignKey: "event"});

db.shift.hasMany(db.strike,     { as: "Strikes",  foreignKey: "shift"});
db.shift.belongsTo(db.category, { as: "Category", foreignKey: "category"});

// Rounds & results
db.round.belongsTo(db.event,        { as: "Event",       foreignKey: "event"});
db.round.belongsTo(db.timeslot,     { as: "Timeslot",    foreignKey: "timeslot"});
db.round.belongsTo(db.site,         { as: "Site",        foreignKey: "site"});
db.round.belongsTo(db.tiebreak_set, { as: "TiebreakSet", foreignKey: "tiebreak_set"});

db.round.hasMany(db.changeLog, { as: "ChangeLogs", foreignKey: "round"});

db.round.belongsToMany( db.rpool, {as: "RPools", foreignKey: "round", through: 'rpool_rounds'});

db.section.belongsTo(db.room,   { as: "Room",    foreignKey: "room"});
db.section.belongsTo(db.round,  { as: "Round",   foreignKey: "round"});
db.section.belongsTo(db.ballot, { as: "Ballot",  foreignKey: "ballot"});
db.section.hasMany(db.changeLog,      { as: 'LogsOld', foreignKey: "old_panel"});
db.section.hasMany(db.changeLog,      { as: 'LogsNew', foreignKey: "new_panel"});

db.ballot.belongsTo(db.entry,   { as: "Entry",   foreignKey: "entry"});
db.ballot.belongsTo(db.judge,   { as: "Judge",   foreignKey: "judge"});
db.ballot.belongsTo(db.section, { as: "Section", foreignKey: "panel"});
db.ballot.belongsTo(db.person,  { as: 'collected_by'});
db.ballot.belongsTo(db.person,  { as: 'entered_by'});
db.ballot.belongsTo(db.person,  { as: 'audited_by'});

db.ballot.hasMany(db.score,    { as: "Scores",  foreignKey: "ballot"});
db.score.belongsTo(db.ballot,  { as: "Ballot",  foreignKey: "ballot"});
db.score.belongsTo(db.student, { as: "Student", foreignKey: "student"});

// Published Results

db.result.belongsTo(db.result_set,   { as: "ResultSet",    foreignKey: "result_set"});
db.result.belongsTo(db.entry,        { as: "Entry",        foreignKey: "entry"});
db.result.belongsTo(db.student,      { as: "Student",      foreignKey: "student"});
db.result.belongsTo(db.school,       { as: "School",       foreignKey: "school"});
db.result.belongsTo(db.round,        { as: "Round",        foreignKey: "round"});

db.result.hasMany(db.result_value,   { as: "ResultValues", foreignKey: "result"});

db.result_set.hasMany(db.result,     { as: "Result", foreignKey: "result_set"});
db.result_set.belongsTo(db.tourn,    { as: "Tourn",  foreignKey: "tourn"});
db.result_set.belongsTo(db.event,    { as: "Event",  foreignKey: "event"});
db.result_value.belongsTo(db.result, { as: "Result", foreignKey: "school"});

// Person & identities

db.person.hasMany(db.login,         { as: "Logins",        foreignKey: "person"});
db.person.hasMany(db.student,       { as: "Students",      foreignKey: "person"});
db.person.hasMany(db.judge,         { as: "Judges",        foreignKey: "person"});
db.person.hasMany(db.chapterJudge, { as: "ChapterJudges", foreignKey: "person"});
db.person.hasMany(db.conflict,      { as: "Conflicts",     foreignKey: "person"});
db.person.hasMany(db.changeLog,           { as: "ChangeLogs",          foreignKey: "person"});
db.person.hasMany(db.permission,    { as: "Permissions",   foreignKey: "person"});

db.person.belongsToMany(db.tourn,   { as: "TournIgnores", foreignKey: "person", through: 'tourn_ignore'});
db.person.belongsToMany(db.tourn,   { as: "Tourns",       foreignKey: "person", through: 'permission'});
db.person.belongsToMany(db.region,  { as: "Regions",      foreignKey: "person", through: 'permission'});
db.person.belongsToMany(db.chapter, { as: "Chapters",     foreignKey: "person", through: 'permission'});
db.person.belongsToMany(db.circuit, { as: "Circuits",     foreignKey: "person", through: 'permission'});

db.conflict.belongsTo(db.person,  { as: "Person", foreignKey: "person"});
db.conflict.belongsTo(db.person,  { as: 'conflicted'});
db.conflict.belongsTo(db.chapter, { as: "Chapter", foreignKey: "chapter"});
db.conflict.belongsTo(db.person,  { as: 'added_by'});

db.login.belongsTo(db.person, { as: "Person", foreignKey: "person"});

//Permissions
db.permission.belongsTo(db.person,   { as: "Person",   foreignKey: "person"});
db.permission.belongsTo(db.district, { as: "District", foreignKey: "district"});
db.permission.belongsTo(db.tourn,    { as: "Tourn",    foreignKey: "tourn"});
db.permission.belongsTo(db.region,   { as: "Region",   foreignKey: "region"});
db.permission.belongsTo(db.chapter,  { as: "Chapter",  foreignKey: "chapter"});
db.permission.belongsTo(db.circuit,  { as: "Circuit",  foreignKey: "circuit"});
db.permission.belongsTo(db.category, { as: "Category", foreignKey: "category"});

// Sweepstakes.  Their very own special world.  That I hate.
db.sweep_set.belongsToMany(db.sweep_set, { as: "Parents",    foreignKey: "sweep_set", through: 'sweep_include'});
db.sweep_set.belongsToMany(db.sweep_set, { as: "Children",   foreignKey: "sweep_set", through: 'sweep_include'});
db.sweep_set.belongsToMany(db.event,     { as: "Events",     foreignKey: "sweep_set", through: 'sweep_events'});
db.sweep_set.belongsTo(db.tourn,         { as: "Tourn",      foreignKey: "tourn"});
db.sweep_set.hasMany(db.sweep_rule,      { as: "SweepRules", foreignKey: "sweep_set"});
db.sweep_rule.belongsTo(db.sweep_set,    { as: "SweepSet",   foreignKey: "sweep_set"});

// Live Updates functions

db.follower.belongsTo(db.person,  { as: 'Person',  foreignKey: "person"});
db.follower.belongsTo(db.judge,   { as: "Judge",   foreignKey: "judge"});
db.follower.belongsTo(db.entry,   { as: "Entry",   foreignKey: "entry"});
db.follower.belongsTo(db.school,  { as: "School",  foreignKey: "school"});
db.follower.belongsTo(db.student, { as: "Student", foreignKey: "student"});
db.follower.belongsTo(db.tourn,   { as: "Tourn",   foreignKey: "tourn"});

// Sessions
db.session.belongsTo(db.person,   { as: 'Su',       foreignKey: 'su'});
db.session.belongsTo(db.person,   { as: 'Person',   foreignKey: 'person'});
db.session.belongsTo(db.tourn,    { as: 'Tourn',    foreignKey: 'tourn'});
db.session.belongsTo(db.event,    { as: 'Event',    foreignKey: 'event'});
db.session.belongsTo(db.category, { as: 'Category', foreignKey: 'category'});

// Initialize the data objects.

db.sequelize = sequelize;
db.Sequelize = Sequelize;

export default db;

//db.Ballot.belongsTo(db.Person, {as: "CollectedBy"});

