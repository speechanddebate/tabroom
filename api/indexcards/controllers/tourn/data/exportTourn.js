// import { showDateTime } from '../../../helpers/common';
import { objectify, arrayify, objectStrip, objectifySettings, objectifyGroupSettings } from '../../../helpers/objectify';

export const backupTourn = {
	GET: async (req, res) => {
		const db = req.db;

		const tournShell  = await db.tourn.findByPk(req.params.tourn_id);
		const tourn = tournShell.dataValues;

		tourn.backup_created = new Date();
		tourn.created_by     = req.session.email;
		tourn.data_format    = '4.0';

		// A bunch of little things that are all at the other end of a simple
		// FK relationship:

		tourn.emails      = objectify(await db.email.findAll({ where: { tourn: tourn.id } }));
		tourn.webpages    = objectify(await db.webpage.findAll({ where: { tourn: tourn.id } }));
		tourn.permissions = objectify(await db.permission.findAll({ where: { tourn: tourn.id } }));
		tourn.fines       = objectify( await db.fine.findAll({ where: { tourn: tourn.id } }));
		tourn.patterns    = objectify( await db.pattern.findAll({ where: { tourn: tourn.id } }));
		tourn.timeslots   = objectify( await db.timeslot.findAll({ where: { tourn: tourn.id } }));

		tourn.settings      = objectifySettings(
			await db.tournSetting.findAll({ where: { tourn: tourn.id } })
		);

		// Circuits have a many to many join table so that's harder
		tourn.circuits = arrayify(await db.sequelize.query(`
			select circuit.id
				from circuit, tourn_circuit tc
				where tc.tourn = :tournId
				and tc.circuit = circuit.id
			order by circuit.abbr
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		}), 'id');

		// Protocols, formerly known as tiebreak sets.
		const protocols = await db.sequelize.query(`
			select protocol.id pid, protocol.name pname,
				tiebreak.*
			from protocol
				left join tiebreak on tiebreak.protocol = protocol.id
			where protocol.tourn = :tournId
			group by tiebreak.id
			order by protocol.id, tiebreak.name
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		tourn.protocols = {};

		protocols.forEach( (protocol) => {
			if (!tourn.protocols[protocol.pid]) {
				tourn.protocols[protocol.pid] = {
					id        : protocol.pid,
					name      : protocol.pname,
					tiebreaks : {},
				};
			}

			tourn.protocols[protocol.pid].tiebreaks[protocol.id] = objectStrip(
				protocol,
				['pid', 'pname', 'timestamp', 'protocol', 'id']
			);
		});

		const rawProtocolSettings = await db.sequelize.query(`
			select ps.*
			from protocol, protocol_setting ps
			where protocol.tourn = :tournId
				and ps.protocol = protocol.id
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		tourn.protocols = objectifyGroupSettings(rawProtocolSettings, 'protocol', tourn.protocols);

		// Room Pools
		tourn.rpools = await db.sequelize.query(`
			select rpool.id, rpool.name, GROUP_CONCAT(distinct rpool_room.room) rooms, GROUP_CONCAT(distinct rpool_round.round) rounds
				from rpool, rpool_room,  rpool_round
			where rpool.tourn = :tournId
				and rpool.id = rpool_room.rpool
				and rpool.id = rpool_round.rpool
			group by rpool.id
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		tourn.rpools.forEach( (rpool)  => {
			rpool.rooms = rpool.rooms.toString().split(',');
			rpool.rounds = rpool.rooms.toString().split(',');
		});

		// Sites

		const sites = await db.sequelize.query(`
			select site.id sid, site.name sname, site.online, room.*
				from site, tourn_site ts, room
			where ts.tourn = :tournId
				and ts.site = site.id
				and site.id = room.site
				and room.deleted = 0
			group by room.id
				order by site.id, room.name
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		tourn.sites = {};

		sites.forEach( (room)  => {
			if (!tourn.sites[room.sid]) {
				tourn.sites[room.sid] = {
					name   : room.sname,
					online : room.online,
					rooms  : {},
				};
			}
			tourn.sites[room.sid].rooms[room.id] = objectStrip(
				room,
				['id', 'sid', 'sname', 'timestamp', 'deleted', 'online', 'building'],
				['ada', 'inactive'],
			);
		});

		// Tournament result sets.  These can be bulky.
		tourn.result_sets = objectify(await db.resultSet.findAll({ where: { tourn: tourn.id } }));

		const resultKeys = await db.sequelize.query(`
			select result_key.*
				from result_set, result_key
			where result_key.result_set = result_set.id
				and result_set.tourn = :tournId
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		resultKeys.forEach( (rkey) => {
			if (!tourn.result_sets[rkey.result_set].keys) {
				tourn.result_sets[rkey.result_set].keys = {};
			}
			tourn.result_sets[rkey.result_set].keys[rkey.id] = objectStrip(
				rkey,
				['result_set', 'timestamp', 'id'],
				['no_sort', 'sort_desc'],
			);
		});

		const results = await db.sequelize.query(`
			select result.*
				from result_set, result
			where result.result_set = result_set.id
				and result_set.tourn = :tournId
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		results.forEach( (result) => {
			if (!tourn.result_sets[result.result_set].results) {
				tourn.result_sets[result.result_set].results = {};
			}
			tourn.result_sets[result.result_set].results[result.id] = objectStrip(
				result,
				['result_set', 'timestamp', 'id'],
			);
		});

		const resultValues = await db.sequelize.query(`
			select result_value.*, result_set.id result_set
				from result_set, result, result_value
			where result.result_set = result_set.id
				and result_set.tourn = :tournId
				and result.id = result_value.result
		`, {
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		resultValues.forEach( (rValue) => {

			if (!tourn.result_sets[rValue.result_set].results[rValue.result].details) {
				tourn.result_sets[rValue.result_set].results[rValue.result].details = {};
			}

			tourn.result_sets[rValue.result_set].results[rValue.result].details[rValue.id] = objectStrip(
				rValue,
				['result_set', 'result', 'timestamp', 'id'],
			);
		});

		// And now the fun parts.  Registration data, which includes schools, entries, etc.  NOT judges in the full tourn dump.
		tourn.schools = objectify( await db.school.findAll({ where: { tourn: tourn.id } }));

		const rawSchoolSettings = await db.sequelize.query(`
			select ps.*
			from school, school_setting ps
			where school.tourn = :tournId
				and ps.school = school.id
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		tourn.schools = objectifyGroupSettings(rawSchoolSettings, 'school', tourn.schools);

		const rawSchoolStudents = await db.sequelize.query(`
			select
				student.id,
				student.first, student.middle, student.last, student.phonetic,
				student.grad_year, student.nsda, student.novice, student.retired,
				student.person, student.chapter,
				school.id school
			from event, entry, entry_student es, student, school
			where event.tourn = :tournId
				and event.id = entry.event
				and entry.id = es.entry
				and es.student = student.id
				and student.chapter = school.chapter
				and school.tourn = event.tourn
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawSchoolStudents.forEach( (student) => {
			const school = tourn.schools[student.school];
			if (!school.students) {
				school.students = {};
			}
			school.students[student.id] = student;
			delete school.students[student.id].chapter;
			delete school.students[student.id].school;
			delete school.students[student.id].id;
		});

		const rawSchoolEntries = await db.sequelize.query(`
			select
				entry.id,
				entry.code, entry.name,
				entry.ada, entry.active, entry.tba, entry.dropped, entry.waitlist, entry.unconfirmed,
				entry.dq,
				entry.created_at, entry.registered_by,
				entry.event, entry.school
			from school, entry
			where school.tourn = :tournId
				and school.id = entry.school
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawSchoolEntries.forEach( (entry) => {
			const school = tourn.schools[entry.school];

			if (!school.entries) {
				school.entries = {};
			}
			school.entries[entry.id] = entry;
			delete school.entries[entry.id].school;
			delete school.entries[entry.id].id;
		});

		const rawEntrySettings = await db.sequelize.query(`
			select
				entry.id entry, entry.school,
				es.tag, es.value, es.value_date, es.value_text
			from (entry, entry_setting es, school)
			where school.tourn = :tournId
				and school.id = entry.school
				and entry.id = es.entry
			group by entry.id
		`,{
			replacements: { tournId: req.params.tourn_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		rawEntrySettings.forEach( (es) => {
			const entry = tourn.schools[es.school].entries[es.entry];

			if (!entry.value) {
				return;
			}

			if (!entry.settings) {
				entry.settings = {};
			}

			if (es.value === 'date') {
				entry.settings[es.tag] = es.value_date;
			} else if (es.value === 'text') {
				entry.settings[es.tag] = es.value_text;
			} else if (es.value === 'json') {
				entry.settings[es.tag] = es.value_json;
			} else if (es.value) {
				entry.settings[es.tag] = es.value;
			}
		});

		// And the real monster: Categories, judges, events, rounds, and results.

		return res.status(200).json(tourn);
	},
};

export const restoreTourn = {
	POST: async (req, res) => {

		const tournData = req.body;

		if (!tournData.tourn_id) {
			tournData.tourn_id = req.params.tourn_id;
		}

		if (!tournData.tourn_id) {
			tournData.tourn_id = req.session.tourn_id;
		}

		// process it later but for now

		return res.status(200).json({ message: 'yay' });
	},
};
