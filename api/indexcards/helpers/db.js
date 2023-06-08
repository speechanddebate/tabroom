import { Sequelize, DataTypes } from 'sequelize';
import fs from 'fs';
import { join } from 'path';
import { errorLogger, debugLogger } from './logger';

import config from '../../config/config';

const sequelize = new Sequelize(
	config.DB_DATABASE,
	config.DB_USER,
	config.DB_PASS,
	config.sequelizeOptions
);

// By default Sequelize wants you to try...catch every single database query
// for Reasons?  Otherwise all your database errors just go unprinted and you
// get a random unfathomable 500 error.  Yeah, because that's great.  This will
// try/catch every query so I don't have to deal.
const errorsPlease = ['findAll', 'findOne', 'save', 'create', 'findByPk'];

const dbError = (err) => {
	debugLogger.error(err);
	process.exit(0);
};

errorsPlease.forEach((dingbat) => {
	const originalFunction = Sequelize[dingbat];
	Sequelize[dingbat] = (...args) => {
		return originalFunction.apply(this, args).catch(err => {
			dbError(err);
		});
	};
});

// Iterate the models directory and load all models dynamically
const models = {};
const files = await fs.promises.readdir('./indexcards/models');
const promises = files
	.filter((file) => {
		return (
			file.indexOf('.') !== 0
			&& file !== 'index.js'
			&& file.slice(-3) === '.js'
		);
	})
	.map((file) => {
		return import(join(process.cwd(), 'indexcards/models', file));
	});

const modules = await Promise.all(promises);
modules.forEach((m) => {
	models[m.default.name] = m.default;
});

const db = {};
Object.keys(models).forEach(m => {
	const model = models[m](sequelize, DataTypes);
	db[model.name] = model;
});

Object.keys(db).forEach( (modelName) => {
	if ('associate' in db[modelName]) {
		db[modelName].associate(db);
	}
});

// Relations, lots and lots of relations
db.ad.belongsTo(db.person, { as : 'Person'     , foreignKey : 'person' });
db.ad.belongsTo(db.person, { as : 'ApprovedBy' , foreignKey : 'approved_by' });

db.autoqueue.belongsTo(db.round  , { as: 'Round'     , foreignKey: 'round' });
db.autoqueue.belongsTo(db.event  , { as: 'Event'     , foreignKey: 'event' });
db.autoqueue.belongsTo(db.person , { as: 'CreatedBy' , foreignKey: 'created_by' });

db.changeLog.belongsTo(db.person   , { as: 'Person'     , foreignKey: 'person' });
db.changeLog.belongsTo(db.tourn    , { as: 'Tourn'      , foreignKey: 'tourn' });
db.changeLog.belongsTo(db.event    , { as: 'Event'      , foreignKey: 'event' });
db.changeLog.belongsTo(db.school   , { as: 'School'     , foreignKey: 'school' });
db.changeLog.belongsTo(db.entry    , { as: 'Entry'      , foreignKey: 'entry' });
db.changeLog.belongsTo(db.judge    , { as: 'Judge'      , foreignKey: 'judge' });

db.changeLog.belongsTo(db.section , { as: 'Section'     , foreignKey: 'panel' });
db.changeLog.belongsTo(db.panel   , { as: 'Panel'       , foreignKey: 'panel' });
db.changeLog.belongsTo(db.round   , { as: 'Round'       , foreignKey: 'round' });
db.changeLog.belongsTo(db.panel   , { as: 'oldPanel'    , foreignKey: 'old_panel' });
db.changeLog.belongsTo(db.panel   , { as: 'newPanel'    , foreignKey: 'new_panel' });
db.changeLog.belongsTo(db.section , { as : 'oldSection' , foreignKey : 'old_panel' });
db.changeLog.belongsTo(db.section , { as : 'newSection' , foreignKey : 'new_panel' });

db.changeLog.belongsTo(db.fine     , { as: 'Fine'       , foreignKey: 'fine' });
db.changeLog.belongsTo(db.category , { as: 'Category'   , foreignKey: 'category' });
db.changeLog.belongsTo(db.circuit  , { as: 'Circuit'    , foreignKey: 'circuit' });

db.campusLog.belongsTo(db.person  , { as: 'Person'  , foreignKey: 'person' });
db.campusLog.belongsTo(db.tourn   , { as: 'Tourn'   , foreignKey: 'tourn' });
db.campusLog.belongsTo(db.panel   , { as: 'Panel'   , foreignKey: 'panel' });
db.campusLog.belongsTo(db.section , { as: 'Section' , foreignKey: 'panel' });
db.campusLog.belongsTo(db.school  , { as: 'School'  , foreignKey: 'school' });
db.campusLog.belongsTo(db.student , { as: 'Student' , foreignKey: 'student' });
db.campusLog.belongsTo(db.entry   , { as: 'Entry'   , foreignKey: 'entry' });
db.campusLog.belongsTo(db.judge   , { as: 'Judge'   , foreignKey: 'judge' });

db.circuit.belongsToMany(db.tourn,   { as: 'Tourns',   foreignKey: 'circuit', through: 'tourn_circuit' });
db.circuit.belongsToMany(db.person,  {
	uniqueKey  : 'id',
	as         : 'Persons',
	foreignKey : 'circuit',
	otherKey   : 'person',
	through    : 'permission',
});
db.circuit.belongsToMany(db.chapter, { as: 'Chapters', foreignKey: 'circuit', through: 'chapter_circuit' });

db.circuit.hasMany(db.file       , { as: 'Files'       , foreignKey: 'circuit' });
db.circuit.hasMany(db.permission , { as: 'Permissions' , foreignKey: 'circuit' });

db.site.belongsTo(db.person,  { as: 'Host',    foreignKey: 'host' });
db.site.belongsTo(db.circuit, { as: 'Circuit', foreignKey: 'circuit' });

db.site.hasMany(db.weekend , { as: 'Weekends' , foreignKey: 'site' });
db.site.hasMany(db.room    , { as: 'Rooms'    , foreignKey: 'site' });
db.site.hasMany(db.round   , { as: 'Rounds'   , foreignKey: 'site' });

db.site.belongsToMany(db.tourn, { through: 'tourn_site' });

db.weekend.belongsTo(db.tourn, { as: 'Tourn', foreignKey: 'tourn' });
db.weekend.belongsTo(db.site, { as: 'Site', foreignKey: 'site' });

db.room.hasMany(db.roomStrike  , { as: 'Strikes' , foreignKey: 'room' });
db.room.belongsTo(db.site      , { as: 'Site'    , foreignKey: 'site' });

db.roomStrike.belongsTo(db.room,  { as: 'Room',  foreignKey: 'room' });
db.roomStrike.belongsTo(db.event, { as: 'Event', foreignKey: 'event' });
db.roomStrike.belongsTo(db.tourn, { as: 'Tourn', foreignKey: 'tourn' });
db.roomStrike.belongsTo(db.entry, { as: 'Entry', foreignKey: 'entry' });
db.roomStrike.belongsTo(db.judge, { as: 'Judge', foreignKey: 'judge' });

db.chapter.belongsTo(db.district, { as: 'District', foreignKey: 'district' });
db.chapter.hasMany(db.student,       { as: 'Students',      foreignKey: 'chapter' });
db.chapter.hasMany(db.permission,    { as: 'Permissions',   foreignKey: 'chapter' });
db.chapter.hasMany(db.school,        { as: 'Schools',       foreignKey: 'chapter' });
db.chapter.hasMany(db.chapterJudge, { as: 'ChapterJudges', foreignKey: 'chapter' });
db.chapter.belongsToMany(db.region,  { as: 'Regions',  foreignKey: 'circuit', through: 'chapter_circuit' });
db.chapter.belongsToMany(db.circuit, { as: 'Circuits', foreignKey: 'circuit', through: 'chapter_circuit' });
db.chapter.belongsToMany(db.person,  {
	uniqueKey  : 'id',
	as         : 'Persons',
	foreignKey : 'chapter',
	otherKey   : 'person',
	through    : 'permission',
});

db.chapterCircuit.belongsTo(db.circuit, { as: 'Circuit', foreignKey: 'circuit' });
db.chapterCircuit.belongsTo(db.chapter, { as: 'Chapter', foreignKey: 'chapter' });
db.chapterCircuit.belongsTo(db.region,  { as: 'Region', foreignKey: 'region' });

db.chapterJudge.hasMany(db.judge,     { as: 'Judges',        foreignKey: 'chapter_judge' });

db.chapterJudge.belongsTo(db.chapter, { as: 'Chapter',       foreignKey: 'chapter' });
db.chapterJudge.belongsTo(db.person,  { as: 'Person',        foreignKey: 'person' });
db.chapterJudge.belongsTo(db.person,  { as: 'PersonRequest', foreignKey: 'person_request' });

db.district.hasMany(db.chapter,    { as: 'Chapters',    foreignKey: 'district' });
db.district.hasMany(db.permission, { as: 'Permissions', foreignKey: 'district' });

db.student.belongsTo(db.chapter, { as: 'Chapter',       foreignKey: 'chapter' });
db.student.belongsTo(db.person,  { as: 'Person',        foreignKey: 'person' });
db.student.belongsTo(db.person,  { as: 'PersonRequest', foreignKey: 'person_request' });

db.student.hasMany(db.score,       { as: 'Scores',    foreignKey: 'student' });
db.student.hasMany(db.follower,    { as: 'Followers', foreignKey: 'student' });

db.student.belongsToMany(db.entry, { as: 'Entries',   foreignKey: 'student', through: 'entry_students' });

db.studentVote.belongsTo(db.panel   , { as: 'Panel'   , foreignKey: 'panel' });
db.studentVote.belongsTo(db.section , { as: 'Section' , foreignKey: 'panel' });
db.studentVote.belongsTo(db.entry   , { as: 'Entry'   , foreignKey: 'entry' });
db.studentVote.belongsTo(db.person  , { as: 'Voter'   , foreignKey: 'voter' });
db.studentVote.belongsTo(db.person  , { as: 'Entered' , foreignKey: 'entered_by' });

db.region.belongsTo(db.circuit,    { as: 'Circuit', foreignKey: 'circuit' });
db.region.belongsTo(db.tourn,      { as: 'Tourn', foreignKey: 'tourn' });

db.region.hasMany(db.strike,     { as: 'Strikes',     foreignKey: 'region' });
db.region.hasMany(db.permission, { as: 'Permissions', foreignKey: 'region' });
db.region.hasMany(db.judgeHire,  { as: 'JudgeHires',  foreignKey: 'region' });
db.region.hasMany(db.fine,       { as: 'Fines',       foreignKey: 'region' });

db.region.belongsToMany(db.chapter, { uniqueKey: 'id', as: 'Chapters', foreignKey: 'region', through: 'chapter_circuit' });
db.region.belongsToMany(db.person,  {
	uniqueKey  : 'id',
	as         : 'Persons',
	foreignKey : 'region',
	otherKey   : 'person',
	through    : 'permission',
});

db.regionSetting.belongsTo(db.region,  { as: 'Region', foreignKey: 'region' });
db.region.hasMany(db.regionSetting,    { as: 'Settings', foreignKey: 'region' });
db.regionSetting.belongsTo(db.setting, { as: 'Setting', foreignKey: 'setting' });

db.topic.belongsTo(db.person, { as: 'Creator', foreignKey: 'created_by' });

// Tournament wide relations
db.tourn.hasMany(db.changeLog  , { as: 'ChangeLogs'  , foreignKey: 'tourn' });
db.tourn.hasMany(db.category   , { as: 'Categories'  , foreignKey: 'tourn' });
db.tourn.hasMany(db.concession , { as: 'Concessions' , foreignKey: 'tourn' });
db.tourn.hasMany(db.email      , { as: 'Emails'      , foreignKey: 'tourn' });
db.tourn.hasMany(db.event      , { as: 'Events'      , foreignKey: 'tourn' });
db.tourn.hasMany(db.entry      , { as: 'Entries'     , foreignKey: 'tourn' });
db.tourn.hasMany(db.file       , { as: 'Files'       , foreignKey: 'tourn' });
db.tourn.hasMany(db.fine       , { as: 'Fines'       , foreignKey: 'tourn' });
db.tourn.hasMany(db.follower   , { as: 'Followers'   , foreignKey: 'tourn' });
db.tourn.hasMany(db.hotel      , { as: 'Hotels'      , foreignKey: 'tourn' });
db.tourn.hasMany(db.region     , { as: 'Regions'     , foreignKey: 'tourn' });
db.tourn.hasMany(db.resultSet  , { as: 'ResultSets'  , foreignKey: 'tourn' });
db.tourn.hasMany(db.school     , { as: 'Schools'     , foreignKey: 'tourn' });
db.tourn.hasMany(db.timeslot   , { as: 'Timeslots'   , foreignKey: 'tourn' });
db.tourn.hasMany(db.tournFee   , { as: 'TournFees'   , foreignKey: 'tourn' });
db.tourn.hasMany(db.protocol   , { as: 'Protocols'   , foreignKey: 'tourn' });
db.tourn.hasMany(db.webpage    , { as: 'Webpages'    , foreignKey: 'tourn' });
db.tourn.hasMany(db.weekend    , { as: 'Weekends'    , foreignKey: 'tourn' });
db.tourn.hasMany(db.permission , { as: 'Permissions' , foreignKey: 'tourn' });
db.tourn.hasMany(db.rpool      , { as: 'RoomPools'   , foreignKey: 'tourn' });

db.tournSite = sequelize.define('tournSite', {
	id: {
		type          : DataTypes.INTEGER,
		primaryKey    : true,
		autoIncrement : true,
		allowNull     : false,
	},
}, {
	tableName: 'tourn_site',
});

db.tourn.belongsToMany(db.site,  {
	as         : 'Sites',
	through    : db.tournSite,
	foreignKey : 'tourn',
	otherKey   : 'site',
});

db.tourn.belongsToMany(db.person,  {
	uniqueKey  : 'id',
	as         : 'Admins',
	foreignKey : 'tourn',
	otherKey   : 'person',
	through    : 'permission',
});

db.tourn.belongsToMany(db.circuit, {
	uniqueKey  : 'id',
	as         : 'Circuits',
	foreignKey : 'tourn',
	otherKey   : 'circuit',
	through    : 'tourn_circuit',
});

db.tournFee.belongsTo(db.tourn , { as: 'Tourn'   , foreignKey: 'tourn' });
db.timeslot.belongsTo(db.tourn , { as: 'Tourn'   , foreignKey: 'tourn' });
db.timeslot.hasMany(db.round   , { as: 'Rounds'  , foreignKey: 'timeslot' });
db.timeslot.hasMany(db.strike  , { as: 'Strikes' , foreignKey: 'timeslot' });

db.category.belongsTo(db.tourn,    { as: 'Tourn',   foreignKey: 'tourn' });
db.category.belongsTo(db.pattern,  { as: 'Pattern', foreignKey: 'pattern' });

db.category.hasMany(db.event,      	 { as: 'Events',        foreignKey: 'category' });
db.category.hasMany(db.judge,      	 { as: 'Judges',        foreignKey: 'category' });
db.category.hasMany(db.changeLog,  	 { as: 'ChangeLogs',    foreignKey: 'category' });
db.category.hasMany(db.permission, 	 { as: 'Permissions',   foreignKey: 'category' });
db.category.hasMany(db.ratingSubset, { as: 'RatingSubsets', foreignKey: 'category' });

db.event.belongsTo(db.tourn,         { as: 'Tourn',        foreignKey: 'tourn' });
db.event.belongsTo(db.category,      { as: 'Category',     foreignKey: 'category' });
db.event.belongsTo(db.pattern,       { as: 'Pattern',      foreignKey: 'pattern' });
db.event.belongsTo(db.ratingSubset,  { as: 'RatingSubset', foreignKey: 'rating_subset' });

db.event.hasMany(db.file,      { as: 'Files',      foreignKey: 'event' });
db.event.hasMany(db.entry,     { as: 'Entries',    foreignKey: 'event' });
db.event.hasMany(db.round,     { as: 'Rounds',     foreignKey: 'event' });
db.event.hasMany(db.changeLog, { as: 'ChangeLogs', foreignKey: 'event' });

db.event.belongsToMany(db.sweepProtocol, { through: 'sweep_events' });

db.pattern.hasMany(db.event,     { as: 'Events',     foreignKey: 'pattern' });
db.pattern.hasMany(db.category,  { as: 'Categories', foreignKey: 'pattern' });
db.pattern.belongsTo(db.pattern, { as: 'Exclude',    foreignKey: 'exclude' });
db.pattern.belongsTo(db.tourn,   { as: 'Tourn',      foreignKey: 'tourn' });

db.tiebreak.belongsTo(db.protocol , { as: 'Protocol' , foreignKey : 'protocol' });
db.tiebreak.belongsTo(db.protocol , { as: 'Child'    , foreignKey : 'child' });

db.protocol.belongsTo(db.tourn  , { as: 'Tourn'     , foreignKey : 'tourn' });
db.protocol.hasMany(db.tiebreak , { as: 'Tiebreaks' , foreignKey : 'protocol' });
db.protocol.hasMany(db.tiebreak , { as: 'Parents'   , foreignKey : 'child' });

db.webpage.belongsTo(db.person  , { as: 'Editor'    , foreignKey : 'last_editor' });
db.webpage.belongsTo(db.webpage , { as: 'Parent'    , foreignKey : 'tourn' });
db.webpage.belongsTo(db.tourn   , { as: 'Tourn'     , foreignKey : 'tourn' });

db.webpage.hasMany(db.file,      { as: 'File',   foreignKey: 'webpage' });
db.webpage.hasMany(db.webpage,   { as: 'Child',  foreignKey: 'parent' } );

db.jpool.belongsTo(db.category , { as: 'Category' , foreignKey: 'category' });
db.jpool.belongsTo(db.site     , { as: 'Site'     , foreignKey: 'site' });
db.jpool.belongsTo(db.jpool    , { as: 'Parent'   , foreignKey: 'parent' });

db.jpool.belongsToMany(db.judge , { through: 'jpool_judge', foreignKey: 'jpool' });
db.jpool.belongsToMany(db.round , { through: 'jpool_round', foreignKey: 'jpool' });

db.rpool.belongsTo(db.tourn, 	 { as: 'Tourn', foreignKey: 'tourn' });
db.rpool.belongsToMany(db.room,  { as: 'Rooms',  foreignKey: 'rpool', through: 'rpool_room' });
db.rpool.belongsToMany(db.round, { as: 'Rounds', foreignKey: 'rpool', through: 'rpool_round' });

// Registration data
db.school.hasMany(db.entry     , { as: 'Entries'    , foreignKey: 'school' });
db.school.hasMany(db.rating    , { as: 'Ratings'    , foreignKey: 'school' });
db.school.hasMany(db.fine      , { as: 'Fines'      , foreignKey: 'school' });
db.school.hasMany(db.strike    , { as: 'Strikes'    , foreignKey: 'school' });
db.school.hasMany(db.changeLog , { as: 'ChangeLogs' , foreignKey: 'school' });
db.school.hasMany(db.file      , { as: 'Files'      , foreignKey: 'school' });
db.school.hasMany(db.judgeHire , { as: 'JudgeHires' , foreignKey: 'school' });
db.school.hasMany(db.judge     , { as: 'Judges'     , foreignKey: 'school' });

db.school.belongsTo(db.tourn    , { as: 'Tourn'   , foreignKey: 'tourn' });
db.school.belongsTo(db.chapter  , { as: 'Chapter' , foreignKey: 'chapter' });
db.school.belongsTo(db.region   , { as: 'Region'  , foreignKey: 'region' });
db.school.belongsTo(db.district , { as: 'District', foreignKey: 'district' });

db.fine.belongsTo(db.person  , { as: 'LeviedBy'  , foreignKey: 'levied_by' });
db.fine.belongsTo(db.person  , { as: 'DeletedBy' , foreignKey: 'deleted_by' });
db.fine.belongsTo(db.tourn   , { as: 'Tourn'     , foreignKey: 'tourn' });
db.fine.belongsTo(db.school  , { as: 'School'    , foreignKey: 'school' });
db.fine.belongsTo(db.region  , { as: 'Region'    , foreignKey: 'region' });
db.fine.belongsTo(db.judge   , { as: 'Judge'     , foreignKey: 'judge' });
db.fine.belongsTo(db.fine    , { as: 'Parent'    , foreignKey: 'parent' });
db.fine.belongsTo(db.invoice , { as: 'Invoice'   , foreignKey: 'invoice' });

db.fine.hasMany(db.changeLog,      { as: 'ChangeLogs',   foreignKey: 'fine' });

db.entry.hasMany(db.studentVote , { as: 'StudentVotes' , foreignKey: 'entry' });
db.entry.hasMany(db.ballot      , { as: 'Ballots'      , foreignKey: 'entry' });
db.entry.hasMany(db.rating      , { as: 'Ratings'      , foreignKey: 'entry' });
db.entry.hasMany(db.strike      , { as: 'Strikes'      , foreignKey: 'entry' });
db.entry.hasMany(db.changeLog   , { as: 'ChangeLogs'   , foreignKey: 'entry' });

db.entry.belongsTo(db.tourn,   { as: 'Tourn',        foreignKey: 'tourn' });
db.entry.belongsTo(db.school,  { as: 'School',       foreignKey: 'school' });
db.entry.belongsTo(db.event,   { as: 'Event',        foreignKey: 'event' });
db.entry.belongsTo(db.person,  { as: 'RegisteredBy', foreignKey: 'registered_by' });

db.entry.belongsToMany(db.student,   {
	uniqueKey  : 'id',
	as         : 'Students',
	foreignKey : 'entry',
	otherKey   : 'student',
	through    : 'entry_student',
});

db.entry.belongsToMany(db.panel,   {
	uniqueKey  : 'id',
	as         : 'Panels',
	foreignKey : 'entry',
	otherKey   : 'panel',
	through    : 'ballot',
});

db.entry.belongsToMany(db.section,   {
	uniqueKey  : 'id',
	as         : 'Sections',
	foreignKey : 'entry',
	otherKey   : 'panel',
	through    : 'ballot',
});

db.judge.hasMany(db.fine      , { as: 'Fines'      , foreignKey: 'judge' });
db.judge.hasMany(db.ballot    , { as: 'Ballots'    , foreignKey: 'judge' });
db.judge.hasMany(db.rating    , { as: 'Ratings'    , foreignKey: 'judge' });
db.judge.hasMany(db.strike    , { as: 'Strikes'    , foreignKey: 'judge' });
db.judge.hasMany(db.changeLog , { as: 'ChangeLogs' , foreignKey: 'judge' });

db.judge.belongsTo(db.school       , { as: 'School'        , foreignKey: 'school' });
db.judge.belongsTo(db.category     , { as: 'Category'      , foreignKey: 'category' });
db.judge.belongsTo(db.category     , { as: 'AltCategory'   , foreignKey: 'alt_category' });
db.judge.belongsTo(db.category     , { as: 'Covers'        , foreignKey: 'covers' });
db.judge.belongsTo(db.chapterJudge , { as: 'ChapterJudge'  , foreignKey: 'chapter_judge' });
db.judge.belongsTo(db.person       , { as: 'Person'        , foreignKey: 'person' });
db.judge.belongsTo(db.person       , { as: 'PersonRequest' , foreignKey: 'person_request' });

db.judge.belongsToMany(db.panel,   {
	uniqueKey  : 'id',
	as         : 'Panels',
	foreignKey : 'judge',
	otherKey   : 'panel',
	through    : 'ballot',
});

db.judge.belongsToMany(db.section,   {
	uniqueKey  : 'id',
	as         : 'Sections',
	foreignKey : 'judge',
	otherKey   : 'panel',
	through    : 'ballot',
});

db.judge.belongsToMany(db.jpool,   {
	uniqueKey  : 'id',
	as         : 'Pools',
	foreignKey : 'judge',
	otherKey   : 'jpool',
	through    : 'jpool_judge',
});

db.judgeHire.belongsTo(db.person,   { as: 'Requestor', foreignKey: 'requestor' });
db.judgeHire.belongsTo(db.judge,    { as: 'Judge',     foreignKey: 'judge' });
db.judgeHire.belongsTo(db.tourn,    { as: 'Tourn',     foreignKey: 'tourn' });
db.judgeHire.belongsTo(db.school,   { as: 'School',    foreignKey: 'school' });
db.judgeHire.belongsTo(db.region,   { as: 'Region',    foreignKey: 'region' });
db.judgeHire.belongsTo(db.category, { as: 'Category',  foreignKey: 'category' });

// Specialty registration data later

db.concession.belongsTo(db.tourn, { as: 'Tourn', foreignKey: 'tourn' });

db.concession.hasMany(db.concessionPurchase, { as: 'Purchases', foreignKey: 'concession' });
db.concession.hasMany(db.concessionType,     { as: 'Types',     foreignKey: 'concession' });

db.concessionType.hasMany(db.concessionOption , { as: 'Options'    , foreignKey: 'concession_type' });
db.concessionType.belongsTo(db.concession     , { as: 'Concession' , foreignKey: 'concession' });

db.concessionPurchase.belongsTo(db.concession, { as: 'Concession', foreignKey: 'concession' });
db.concessionPurchase.belongsTo(db.school,     { as: 'School',     foreignKey: 'school' });

db.concessionPurchase.belongsToMany( db.concessionOption,
	{ as: 'Options', foreignKey: 'concession_option', through: 'concession_purchase_option' }
);

db.concessionOption.belongsTo(db.concessionType, { as: 'Type', foreignKey:'concession_type' });

db.concessionOption.belongsToMany( db.concessionPurchase,
	{ as: 'Purchases', foreignKey: 'concession_purchase', through: 'concession_purchase_option' }
);

db.email.belongsTo(db.person,  { as: 'Sender',  foreignKey: 'sender' });
db.email.belongsTo(db.tourn,   { as: 'Tourn',   foreignKey: 'tourn' });
db.email.belongsTo(db.circuit, { as: 'Circuit', foreignKey: 'circuit' });

db.file.belongsTo(db.tourn   , { as: 'Tourn'   , foreignKey: 'tourn' });
db.file.belongsTo(db.school  , { as: 'School'  , foreignKey: 'school' });
db.file.belongsTo(db.event   , { as: 'Event'   , foreignKey: 'event' });
db.file.belongsTo(db.circuit , { as: 'Circuit' , foreignKey: 'circuit' });
db.file.belongsTo(db.person  , { as: 'Person'  , foreignKey: 'person' });

db.file.belongsTo(db.file    , { as: 'Parent'  , foreignKey: 'parent' });
db.file.hasMany(db.file , { as: 'Children' , foreignKey: 'parent' });
db.hotel.belongsTo(db.tourn, { as: 'Tourn', foreignKey: 'tourn' });

db.housing.belongsTo(db.person,  { as: 'Requestor', foreignKey: 'requestor' });
db.housing.belongsTo(db.tourn,   { as: 'Tourn',     foreignKey: 'tourn' });
db.housing.belongsTo(db.student, { as: 'Student',   foreignKey: 'student' });
db.housing.belongsTo(db.judge,   { as: 'Judge',     foreignKey: 'judge' });
db.housing.belongsTo(db.school,  { as: 'School',    foreignKey: 'school' });

db.housingSlots.belongsTo(db.tourn, { as: 'Tourn', foreignKey: 'tourn' });
db.invoice.belongsTo(db.school,  { as: 'School',    foreignKey: 'school' });

// Pref Sheets
db.rating.belongsTo(db.tourn,        { as: 'Tourn',        foreignKey: 'tourn' });
db.rating.belongsTo(db.school,       { as: 'School',       foreignKey: 'school' });
db.rating.belongsTo(db.entry,        { as: 'Entry',        foreignKey: 'entry' });
db.rating.belongsTo(db.ratingTier,   { as: 'RatingTier',   foreignKey: 'rating_tier' });
db.rating.belongsTo(db.judge,        { as: 'Judge',        foreignKey: 'judge' });
db.rating.belongsTo(db.ratingSubset, { as: 'RatingSubset', foreignKey: 'rating_subset' });

db.ratingTier.hasMany(db.rating,         { as: 'Ratings',      foreignKey: 'rating_tier' });
db.ratingTier.belongsTo(db.category,     { as: 'Category',     foreignKey: 'category' });
db.ratingTier.belongsTo(db.ratingSubset, { as: 'RatingSubset', foreignKey: 'rating_subset' });

db.ratingSubset.hasMany(db.event,        { as: 'Events',       foreignKey: 'rating_subset' });
db.ratingSubset.hasMany(db.rating,       { as: 'Ratings',      foreignKey: 'rating_subset' });
db.ratingSubset.hasMany(db.ratingTier,   { as: 'RatingTiers',  foreignKey: 'rating_subset' });
db.ratingSubset.belongsTo(db.category,   { as: 'Category',     foreignKey: 'category' });

db.strike.belongsTo(db.tourn,    { as: 'Tourn',     foreignKey: 'tourn' });
db.strike.belongsTo(db.judge,    { as: 'Judge',     foreignKey: 'judge' });
db.strike.belongsTo(db.event,    { as: 'Event',     foreignKey: 'event' });
db.strike.belongsTo(db.entry,    { as: 'Entry',     foreignKey: 'entry' });
db.strike.belongsTo(db.school,   { as: 'School',    foreignKey: 'school' });
db.strike.belongsTo(db.region,   { as: 'Region',    foreignKey: 'region' });
db.strike.belongsTo(db.district, { as: 'District',  foreignKey: 'district' });
db.strike.belongsTo(db.timeslot, { as: 'Timeslot',  foreignKey: 'timeslot' });
db.strike.belongsTo(db.person,   { as: 'EnteredBy', foreignKey: 'entered_by' });
db.strike.belongsTo(db.shift,    { as: 'Shift',     foreignKey: 'shift' });

db.shift.hasMany(db.strike,     { as: 'Strikes',  foreignKey: 'shift' });
db.shift.belongsTo(db.category, { as: 'Category', foreignKey: 'category' });

// Rounds & results
db.round.belongsTo(db.event     , { as: 'Event'     , foreignKey: 'event' });
db.round.belongsTo(db.timeslot  , { as: 'Timeslot'  , foreignKey: 'timeslot' });
db.round.belongsTo(db.site      , { as: 'Site'      , foreignKey: 'site' });
db.round.belongsTo(db.protocol  , { as: 'Protocol'  , foreignKey: 'protocol' });
db.round.belongsTo(db.round     , { as: 'Runoff'    , foreignKey: 'runoff' });
db.round.belongsToMany(db.rpool , { as: 'RoomPools' , foreignKey: 'round'        , through: 'rpool_round' });
db.round.hasMany(db.panel       , { as: 'Panels'    , foreignKey: 'round' });
db.round.hasMany(db.section     , { as: 'Sections'  , foreignKey: 'round' });

db.panel.hasMany(db.ballot     , { as: 'Ballots' , foreignKey: 'panel' });
db.section.hasMany(db.ballot   , { as: 'Ballots' , foreignKey: 'panel' });
db.ballot.belongsTo(db.panel   , { as: 'Panel'   , foreignKey: 'panel' });
db.ballot.belongsTo(db.section , { as: 'Section' , foreignKey: 'panel' });

db.panel.hasMany(db.changeLog   , { as: 'LogsOld'      , foreignKey: 'old_panel' });
db.panel.hasMany(db.changeLog   , { as: 'LogsNew'      , foreignKey: 'new_panel' });
db.panel.hasMany(db.studentVote , { as: 'StudentVotes' , foreignKey: 'panel' });
db.panel.belongsTo(db.room      , { as: 'Room'         , foreignKey: 'room' });
db.panel.belongsTo(db.round     , { as: 'Round'        , foreignKey: 'round' });

db.section.hasMany(db.changeLog   , { as: 'LogsOld'      , foreignKey: 'old_panel' });
db.section.hasMany(db.changeLog   , { as: 'LogsNew'      , foreignKey: 'new_panel' });
db.section.hasMany(db.studentVote , { as: 'StudentVotes' , foreignKey: 'panel' });
db.section.belongsTo(db.room      , { as: 'Room'         , foreignKey: 'room' });
db.section.belongsTo(db.round     , { as: 'Round'        , foreignKey: 'round' });

db.ballot.belongsTo(db.entry  , { as: 'Entry'     , foreignKey: 'entry' });
db.ballot.belongsTo(db.judge  , { as: 'Judge'     , foreignKey: { name: 'judge', allowNull: false, defaultValue: 0 } } );
db.ballot.belongsTo(db.person , { as: 'EnteredBy' , foreignKey: 'entered_by' });
db.ballot.belongsTo(db.person , { as: 'StartedBy' , foreignKey: 'started_by' });
db.ballot.belongsTo(db.person , { as: 'AuditedBy' , foreignKey: 'audited_by' });
db.ballot.hasMany(db.score    , { as: 'Scores'    , foreignKey: 'ballot' });

db.score.belongsTo(db.ballot,  { as: 'Ballot',  foreignKey: 'ballot' });
db.score.belongsTo(db.student, { as: 'Student', foreignKey: 'student' });

// Published Results

db.result.belongsTo(db.resultSet , { as: 'ResultSet'    , foreignKey: 'result_set' });
db.result.belongsTo(db.entry     , { as: 'Entry'        , foreignKey: 'entry' });
db.result.belongsTo(db.student   , { as: 'Student'      , foreignKey: 'student' });
db.result.belongsTo(db.school    , { as: 'School'       , foreignKey: 'school' });
db.result.belongsTo(db.round     , { as: 'Round'        , foreignKey: 'round' });
db.result.belongsTo(db.panel     , { as: 'Panel'        , foreignKey: 'panel' });
db.result.belongsTo(db.section   , { as: 'Section'      , foreignKey: 'panel' });
db.result.hasMany(db.resultValue , { as: 'ResultValues' , foreignKey: 'result' });

db.resultKey.belongsTo(db.resultSet , { as: 'ResultKey' , foreignKey: 'result_set' });

db.resultSet.hasMany(db.result          , { as: 'Result'        , foreignKey: 'result_set' });
db.resultSet.belongsTo(db.tourn         , { as: 'Tourn'         , foreignKey: 'tourn' });
db.resultSet.belongsTo(db.event         , { as: 'Event'         , foreignKey: 'event' });
db.resultSet.belongsTo(db.sweepProtocol , { as: 'SweepProtocol' , foreignKey: 'sweep_set' });
db.resultSet.belongsTo(db.sweepAward    , { as: 'SweepAward'    , foreignKey: 'sweep_award' });

db.resultValue.belongsTo(db.result    , { as: 'Result'    , foreignKey: 'result' });
db.resultValue.belongsTo(db.resultKey , { as: 'ResultKey' , foreignKey: 'result_key' });
db.resultValue.belongsTo(db.protocol  , { as: 'Protocol'  , foreignKey: 'protocol' });

// Person & identities

db.person.hasMany(db.student,       { as: 'Students',      foreignKey: 'person' });
db.person.hasMany(db.judge,         { as: 'Judges',        foreignKey: 'person' });
db.person.hasMany(db.chapterJudge,  { as: 'ChapterJudges', foreignKey: 'person' });
db.person.hasMany(db.conflict,      { as: 'Conflicts',     foreignKey: 'person' });
db.person.hasMany(db.changeLog,     { as: 'ChangeLogs',    foreignKey: 'person' });
db.person.hasMany(db.permission,    { as: 'Permissions',   foreignKey: 'person' });
db.person.hasMany(db.studentVote,   { as: 'StudentVotes',  foreignKey: 'voter' });
db.person.hasMany(db.caseList,      { as: 'CaseLists',     foreignKey: 'person' });

db.person.belongsToMany(db.tourn,   { as: 'IgnoredTourns', foreignKey: 'person', through: 'tourn_ignore' });
db.person.belongsToMany(db.tourn,   {
	uniqueKey  : 'id',
	as         : 'Tourns',
	foreignKey : 'person',
	otherKey   : 'tourn',
	through    : 'permission',
});

db.person.belongsToMany(db.region,  {
	uniqueKey  : 'id',
	as         : 'Regions',
	foreignKey : 'person',
	otherKey   : 'region',
	through    : 'permission',
});

db.person.belongsToMany(db.chapter, {
	uniqueKey  : 'id',
	as         : 'Chapters',
	foreignKey : 'person',
	otherKey   : 'chapter',
	through    : 'permission',
});

db.person.belongsToMany(db.circuit, {
	uniqueKey  : 'id',
	as         : 'Circuits',
	foreignKey : 'person',
	otherKey   : 'circuit',
	through    : 'permission',
});

db.caseList.belongsTo(db.person, { as: 'Person' , foreignKey: 'person' });
db.caseList.belongsTo(db.person, { as: 'Partner', foreignKey: 'partner' });

db.conflict.belongsTo(db.person,  { as: 'Person'     , foreignKey: 'person' });
db.conflict.belongsTo(db.person,  { as: 'Conflicted' , foreignKey: 'conflicted' });
db.conflict.belongsTo(db.chapter, { as: 'Chapter'    , foreignKey: 'chapter' });
db.conflict.belongsTo(db.person,  { as: 'AddedBy'    , foreignKey: 'added_by' });

// Permissions

db.permission.belongsTo(db.person,   { as: 'Person',   foreignKey: 'person' });
db.permission.belongsTo(db.district, { as: 'District', foreignKey: 'district' });
db.permission.belongsTo(db.tourn,    { as: 'Tourn',    foreignKey: 'tourn' });
db.permission.belongsTo(db.region,   { as: 'Region',   foreignKey: 'region' });
db.permission.belongsTo(db.chapter,  { as: 'Chapter',  foreignKey: 'chapter' });
db.permission.belongsTo(db.circuit,  { as: 'Circuit',  foreignKey: 'circuit' });
db.permission.belongsTo(db.category, { as: 'Category', foreignKey: 'category' });

// Sweepstakes.  Their very own special world.  That I hate.
db.sweepProtocol.belongsToMany(db.sweepProtocol,
	{ as: 'Parents',  foreignKey: 'sweep_set', through: 'sweep_include' }
);
db.sweepProtocol.belongsToMany(db.sweepProtocol,
	{ as: 'Children', foreignKey: 'sweep_set', through: 'sweep_include' }
);
db.sweepProtocol.belongsToMany(db.event,
	{ as: 'Events',   foreignKey: 'sweep_set', through: 'sweep_events' }
);

db.sweepProtocol.belongsTo(db.sweepAward, { as: 'SweepAward', foreignKey: 'sweep_award' });
db.sweepProtocol.belongsTo(db.tourn     , { as: 'Tourn'     , foreignKey: 'tourn' });
db.sweepProtocol.hasMany(db.sweepRule   , { as: 'SweepRules', foreignKey: 'sweep_set' });

db.sweepEvent.belongsTo(db.sweepProtocol , { as: 'SweepProtocol' , foreignKey : 'sweep_set' });
db.sweepEvent.belongsTo(db.event         , { as: 'Event'         , foreignKey : 'event' });
db.sweepEvent.belongsTo(db.nsdaCategory  , { as: 'NSDACategory'  , foreignKey : 'nsda_category' });

db.sweepAward.belongsTo(db.circuit, { as: 'Circuit', foreignKey: 'circuit' });

db.sweepRule.belongsTo(db.sweepProtocol , { as: 'SweepProtocol' , foreignKey: 'sweep_set' });
db.sweepRule.belongsTo(db.protocol      , { as: 'Protocol'      , foreignKey: 'protocol' });

// Live Updates functions
db.follower.belongsTo(db.person  , { as: 'Person'   , foreignKey: 'person' });
db.follower.belongsTo(db.person  , { as: 'Follower' , foreignKey: 'follower' });
db.follower.belongsTo(db.judge   , { as: 'Judge'    , foreignKey: 'judge' });
db.follower.belongsTo(db.entry   , { as: 'Entry'    , foreignKey: 'entry' });
db.follower.belongsTo(db.school  , { as: 'School'   , foreignKey: 'school' });
db.follower.belongsTo(db.student , { as: 'Student'  , foreignKey: 'student' });
db.follower.belongsTo(db.tourn   , { as: 'Tourn'    , foreignKey: 'tourn' });

// Sessions
db.session.belongsTo(db.person, { as: 'Su',     foreignKey: 'su' });
db.session.belongsTo(db.person, { as: 'Person', foreignKey: 'person' });

db.setting.hasMany(db.settingLabel   , { as: 'Labels'  , foreignKey: 'setting' });
db.settingLabel.belongsTo(db.setting , { as: 'Setting' , foreignKey: 'setting' });

db.protocolSetting.belongsTo(db.protocol , { as: 'Protocol' , foreignKey : 'protocol' });
db.tournSetting.belongsTo(db.tourn       , { as: 'Tourn'    , foreignKey : 'tourn' });
db.eventSetting.belongsTo(db.event       , { as: 'Event'    , foreignKey : 'event' });
db.roundSetting.belongsTo(db.round       , { as: 'Round'    , foreignKey : 'round' });
db.panelSetting.belongsTo(db.panel       , { as: 'Panel'    , foreignKey : 'panel' });
db.sectionSetting.belongsTo(db.section   , { as: 'Section'  , foreignKey : 'panel' });
db.schoolSetting.belongsTo(db.school     , { as: 'School'   , foreignKey : 'school' });
db.chapterSetting.belongsTo(db.chapter   , { as: 'Chapter'  , foreignKey : 'chapter' });
db.categorySetting.belongsTo(db.category , { as: 'Category' , foreignKey : 'category' });
db.circuitSetting.belongsTo(db.circuit   , { as: 'Circuit'  , foreignKey : 'circuit' });
db.entrySetting.belongsTo(db.entry       , { as: 'Entry'    , foreignKey : 'entry' });
db.jpoolSetting.belongsTo(db.jpool       , { as: 'JPool'    , foreignKey : 'jpool' });
db.judgeSetting.belongsTo(db.judge       , { as: 'Judge'    , foreignKey : 'judge' });
db.personSetting.belongsTo(db.person     , { as: 'Person'   , foreignKey : 'person' });
db.rpoolSetting.belongsTo(db.rpool       , { as: 'RoomPool' , foreignKey : 'rpool' });
db.studentSetting.belongsTo(db.student   , { as: 'Student'  , foreignKey : 'student' });

db.setting.hasMany(db.protocolSetting , { as: 'protocols'  , foreignKey : 'setting' });
db.setting.hasMany(db.tournSetting    , { as: 'tourns'     , foreignKey : 'setting' });
db.setting.hasMany(db.eventSetting    , { as: 'events'     , foreignKey : 'setting' });
db.setting.hasMany(db.roundSetting    , { as: 'rounds'     , foreignKey : 'setting' });
db.setting.hasMany(db.panelSetting    , { as: 'panels'     , foreignKey : 'setting' });
db.setting.hasMany(db.sectionSetting  , { as: 'sections'   , foreignKey : 'setting' });
db.setting.hasMany(db.schoolSetting   , { as: 'schools'    , foreignKey : 'setting' });
db.setting.hasMany(db.chapterSetting  , { as: 'chapters'   , foreignKey : 'setting' });
db.setting.hasMany(db.categorySetting , { as: 'categories' , foreignKey : 'setting' });
db.setting.hasMany(db.circuitSetting  , { as: 'circuits'   , foreignKey : 'setting' });
db.setting.hasMany(db.entrySetting    , { as: 'entries'    , foreignKey : 'setting' });
db.setting.hasMany(db.jpoolSetting    , { as: 'jpools'     , foreignKey : 'setting' });
db.setting.hasMany(db.judgeSetting    , { as: 'judges'     , foreignKey : 'setting' });
db.setting.hasMany(db.personSetting   , { as: 'persons'    , foreignKey : 'setting' });
db.setting.hasMany(db.rpoolSetting    , { as: 'rpools'     , foreignKey : 'setting' });
db.setting.hasMany(db.studentSetting  , { as: 'students'   , foreignKey : 'setting' });

db.protocol.hasMany(db.protocolSetting , { as: 'Settings' , foreignKey : 'protocol' });
db.tourn.hasMany(db.tournSetting       , { as: 'Settings' , foreignKey : 'tourn' });
db.event.hasMany(db.eventSetting       , { as: 'Settings' , foreignKey : 'event' });
db.round.hasMany(db.roundSetting       , { as: 'Settings' , foreignKey : 'round' });
db.panel.hasMany(db.panelSetting       , { as: 'Settings' , foreignKey : 'panel' });
db.section.hasMany(db.sectionSetting   , { as: 'Settings' , foreignKey : 'panel' });
db.school.hasMany(db.schoolSetting     , { as: 'Settings' , foreignKey : 'school' });
db.chapter.hasMany(db.chapterSetting   , { as: 'Settings' , foreignKey : 'chapter' });
db.category.hasMany(db.categorySetting , { as: 'Settings' , foreignKey : 'category' });
db.circuit.hasMany(db.circuitSetting   , { as: 'Settings' , foreignKey : 'circuit' });
db.entry.hasMany(db.entrySetting       , { as: 'Settings' , foreignKey : 'entry' });
db.jpool.hasMany(db.jpoolSetting       , { as: 'Settings' , foreignKey : 'jpool' });
db.judge.hasMany(db.judgeSetting       , { as: 'Settings' , foreignKey : 'judge' });
db.person.hasMany(db.personSetting     , { as: 'Settings' , foreignKey : 'person' });
db.rpool.hasMany(db.rpoolSetting       , { as: 'Settings' , foreignKey : 'rpool' });
db.student.hasMany(db.studentSetting   , { as: 'Settings' , foreignKey : 'student' });

// a standard getter for Tabroom objects that have settings because Palmer is
// teh lazy

db.summon = async (dbTable, objectId) => {

	const options = {};

	// automatically include settings if the model has them.
	if (dbTable.associations.Settings) {
		options.include = 'Settings';
	}

	let dbObject = {};

	try {
		dbObject = await dbTable.findByPk(
			objectId,
			options
		);
	} catch (err) {
		errorLogger.info(`SUMMON QUERY RETURNED ERROR: ${err} for model ${dbTable} PK ${objectId}`);
		return;
	}

	if (!dbObject) {
		errorLogger.info(`NOTHING FOUND: No ${dbTable} record found with key ${objectId}`);
		return;
	}

	const dbData = dbObject.get({ plain: true });
	dbData.table = dbTable.name;

	if (dbData.Settings) {
		dbData.settings = {};

		dbData.Settings
			.sort((a, b) => { return (a.tag > b.tag) ? 1 : -1; })
			.forEach( (item) => {
				if (item.value === 'date' && item.value_date) {
					if (item.value_date !== null) {
						dbData.settings[item.tag] = item.value_date;
					}
				} else if (item.value === 'json') {
					if (item.value_text !== null) {
						dbData.settings[item.tag] = JSON.parse(item.value_text);
					}
				} else if (item.value === 'text') {
					if (item.value_text !== null) {
						dbData.settings[item.tag] = item.value_text;
					}
				} else {
					dbData.settings[item.tag] = item.value;
				}
			});

		delete dbData.Settings;
	}

	return dbData;
};

// And hell, if we're going to be grabbing settings that way might as well build
// a standard way to then change them.

db.setting = async (origin, settingTag, settingValue) => {

	if (origin.table === undefined) {
		return;
	}

	const setKey = `${origin.table}Setting`;

	if (typeof settingValue !== 'undefined') {

		if (settingValue === 0 || settingValue === '' || settingValue === null) {
			await db[setKey].destroy({
				where: { [origin.table] : origin.id, tag: settingTag },
			});
			return;
		}

		await db[setKey].findOrCreate({
			where: { [origin.table] : origin.id, tag: settingTag },
		});

		const updateTo = {};
		let returnValue = '';

		if (typeof (settingValue) === 'object') {
			if (settingValue.text) {
				updateTo.value = 'text';
				updateTo.value_text = settingValue.text;
				returnValue = settingValue.text;
			} else if (settingValue.date) {
				const newDate = new Date(settingValue.date);
				updateTo.value = 'date';
				updateTo.value_date = newDate;
				returnValue = newDate;
			} else if (settingValue.json) {
				updateTo.value = 'json';
				updateTo.value_text = JSON.stringify(settingValue.json);
				returnValue = settingValue.json;
			}
		} else {
			updateTo.value = settingValue;
			returnValue = settingValue;
		}

		await db[setKey].update(
			updateTo,
			{ where: { [origin.table] : origin.id, tag: settingTag } }
		);

		return returnValue;
	}

	const setValue = await db[setKey].findOne({
		where: {
			[origin.table]: origin.id,
			tag: settingTag,
		},
	});

	if (setValue) {

		const settingResult = setValue.get({ plain: true });

		if (settingResult.value === 'json') {
			if (settingResult.value_text) {
				return JSON.parse(settingResult.value_text);
			}
		} else if (settingResult.value === 'date') {
			if (settingResult.value_date) {
				return settingResult.value_date;
			}
		} else if (settingResult.value === 'text') {
			if (settingResult.value_text) {
				return settingResult.value_text;
			}
		} else if (settingResult.value) {
			return settingResult.value;
		}
	}
};

// Initialize the data objects.
db.sequelize = sequelize;
db.Sequelize = Sequelize;

export default db;
