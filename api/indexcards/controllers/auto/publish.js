export const autoPublish = {
	GET: async (req, res) => {

		const db = req.db;

		await db.sequelize.query(`
		 	update round, event_setting autopublish
				set round.post_primary = 3
			where round.event = autopublish.event
				and autopublish.tag = 'autopublish_results'
				and autopublish.value > 0
				and round.start_time > NOW() - INTERVAL 1 DAY
				and (round.post_primary != 3 OR round.post_primary IS NULL)

			and not exists (
				select ballot.id
					from ballot, panel
				where ballot.panel = panel.id
					and panel.round = round.id
					and ballot.audit != 1
					and ballot.bye != 1
					and panel.bye != 1
			)

			and exists (
				select b2.id
					from ballot b2, panel
				where b2.panel = panel.id
					and panel.round = round.id
			)

			and exists (
				select b3.entry
					from ballot b3, panel, round r2
				where r2.event = round.event
					and r2.name = (round.name + 1)
					and r2.id = panel.round
					and panel.id = b3.panel
			)
		`, {
			type : db.sequelize.QueryTypes.UPDATE,
		});

		await db.sequelize.query(`
		 	update round, event_setting autopublish
				set round.post_feedback = 2
			where round.event = autopublish.event
				and autopublish.tag = 'autopublish_results'
				and autopublish.value > 1
				and round.start_time > NOW() - INTERVAL 1 DAY
				and (round.post_feedback != 2 OR round.post_feedback IS NULL)

			and not exists (
				select ballot.id
					from ballot, panel
				where ballot.panel = panel.id
					and panel.round = round.id
					and ballot.audit != 1
					and ballot.bye != 1
					and panel.bye != 1
			)
			and exists (
				select ballot.entry
					from ballot, panel, round r2
				where r2.event = round.event
					and r2.name = (round.name + 1)
					and r2.id = panel.round
					and panel.id = ballot.panel
			)
		`, {
			type : db.sequelize.QueryTypes.UPDATE,
		});

		await db.sequelize.query(`
		 	update round, event_setting autopublish
				set round.post_secondary = 3
			where round.event = autopublish.event
				and autopublish.tag = 'autopublish_results'
				and autopublish.value  = '2'
				and round.start_time > NOW() - INTERVAL 1 DAY
				and (round.post_secondary != 3 OR round.post_secondary IS NULL)

			and not exists (
				select ballot.id
					from ballot, panel
				where ballot.panel = panel.id
					and panel.round = round.id
					and ballot.audit != 1
					and ballot.bye != 1
					and panel.bye != 1
			)
			and exists (
				select ballot.entry
					from ballot, panel, round r2
				where r2.event = round.event
					and r2.name = (round.name + 1)
					and r2.id = panel.round
					and panel.id = ballot.panel
			)
		`, {
			type : db.sequelize.QueryTypes.UPDATE,
		});

		res.status(200).json({
			error   : false,
			message : 'All autopublish rounds updated',
		});
	},
};

export default autoPublish;
