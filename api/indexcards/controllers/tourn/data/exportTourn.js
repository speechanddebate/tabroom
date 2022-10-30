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

		tourn.emails        = objectify(await db.email.findAll({ where: { tourn: tourn.id } }));
		tourn.webpages      = objectify(await db.webpage.findAll({ where: { tourn: tourn.id } }));

		tourn.permissions   = objectify(await db.permission.findAll({ where: { tourn: tourn.id } }));
		tourn.fines         = objectify(await db.fine.findAll({ where: { tourn: tourn.id } }));
		tourn.patterns      = objectify(await db.pattern.findAll({ where: { tourn: tourn.id } }));
		tourn.timeslots     = objectify(await db.timeslot.findAll({ where: { tourn: tourn.id } }));
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
					settings  : {},
				};
			}

			tourn.protocols[protocol.pid].tiebreaks[protocol.id] = objectStrip(
				protocol,
				['pid', 'pname', 'timestamp', 'protocol', 'tiebreak_set', 'id']
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

		const protocolSettings = objectifyGroupSettings(rawProtocolSettings, 'protocol');

		Object.keys(protocolSettings).forEach( (protocolId) => {
			console.log(protocolId);
			tourn.protocols[protocolId].settings = protocolSettings[protocolId];
		});

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
			tourn.sites[room.sid].rooms[room.id] = objectStrip(room, ['id', 'sid', 'sname', 'timestamp', 'deleted', 'online']);
		});

		// Tournament result sets.  These can be bulky.
		tourn.result_sets = objectify(await db.resultSet.findAll({ where: { tourn: tourn.id } }));

		// And now the fun parts.  Registration data, which includes schools, entries, etc.  NOT judges in the full tourn dump.

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
