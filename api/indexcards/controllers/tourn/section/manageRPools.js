// import { showDateTime } from '../../../helpers/common';

export const addRoundToRPool = {

	POST: async (req, res) => {

		const db = req.db;

		const checkRoundQuery = `select
				rpr.id
			from rpool_round rpr
				where rpr.rpool = :rpoolId
				and rpr.round = :roundId
		`;

		const already = await db.sequelize.query(checkRoundQuery,
			{ replacements : {
				rpoolId    : req.params.rpoolId,
				roundId    : parseInt(req.body.property_value),
			},
			type: req.db.sequelize.QueryTypes.SELECT,
			}
		);

		if (already.length > 0) {
			res.status(200).json({
				error: true,
				message : 'Round is already in that room pool',
			});
		}

		const addRoundQuery = `insert into rpool_round (round, rpool) values (:roundId, :rpoolId)`;

		await db.sequelize.query(addRoundQuery,
			{ replacements   : {
				rpoolId  : req.params.rpoolId,
				roundId    : parseInt(req.body.property_value),
			},
			type: req.db.sequelize.QueryTypes.INSERT,
			}
		);

		const allRoundInfo = await db.sequelize.query(`
			select round.id round, round.name, round.label, event.abbr, rpr.id rpr
				from round, event, rpool_round rpr
			where round.id = :roundId
				and event.id = round.event
				and rpr.round = round.id
				and rpr.rpool = :rpoolId
		`, {
			replacements : {
				rpoolId  : req.params.rpoolId,
				roundId  : parseInt(req.body.property_value),
			},
			type: req.db.sequelize.QueryTypes.SELECT,
		});

		const roundInfo = allRoundInfo.shift();

		res.status(200).json({
			reply: `<span class="quarter nospace">
					<a value="1" 
						id="${roundInfo.rpr}"
						property_name="delete"
						target_id="${roundInfo.rpr}"
						on_success="destroy"
						onClick="postSwitch(this, 'rpool_round_rm.mhtml'); fixVisual();"
						class="full white nowrap hover marno smallish"
						title="Remove this round">${roundInfo.abbr} ${roundInfo.name}</a>
				</span>`,
			reply_append : `${req.params.rpoolId}_rounds`,
			error        : false,
			message      : `Round ${roundInfo.name} added to pool`,
		});
	},
};

export default addRoundToRPool;
