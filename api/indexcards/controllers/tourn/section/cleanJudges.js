// import { showDateTime } from '../../../helpers/common';

const getPanelData = async (db, panelId) => {

	const [[settings]] = await db.sequelize.query(`
		select
			round.id round, round.type roundType,
			event.id event, event.type eventType,
			category.id category,
			jpool.id jpool,
			neutrals.value neutrals,
			auto_conflict_hires.value autoConflictHires,
			no_first_years.value noFirstYears,
			conflict_judges.value conflictJudges,
			repeat_judges.value repeatJudges,
			panel_judges.value panelJudges,
			prefs.value prefs,
			tab_ratings.value tabRatings

		from (round, event, category)

			left join category_setting tab_ratings 
				on tab_ratings.category = category.id 
				and tab_ratings.tag = "tab_ratings"
				
			left join category_setting prefs 
				on prefs.category = category.id 
				and prefs.tag = "prefs"
				
			left join category_setting neutrals 
				on neutrals.category = category.id 
				and neutrals.tag = "neutrals"

			left join category_setting auto_conflict_hires 
				on auto_conflict_hires.category = category.id 
				and auto_conflict_hires.tag = "auto_conflict_hires"

			left join event_setting no_first_years
				on no_first_years.event = event.id
				and no_first_years.tag = "no_first_years"
				
			left join event_setting conflict_judges
				on conflict_judges.event = event.id
				and conflict_judges.tag = "conflict_judges"
				
			left join event_setting repeat_judges
				on repeat_judges.event = event.id
				and repeat_judges.tag = "repeat_judges"
				
			left join event_setting panel_judges
				on panel_judges.event = event.id
				and panel_judges.tag = "panel_judges"

			left join jpool_round jpr on jpr.round = round.id
			left join jpool on jpr.jpool = jpool.id
			
		where round.id = :panel
			and round.event = event.id
			and event.category = category.id
	`, {
		replacements: { panel: panelId },
		type: db.sequelize.QueryTypes.SELECT,
	});

	return settings;
};

const getPanelJudges = async (db, panel) => {

	let judgeQuery = '';
	const replacements = {};

	if (panel.jpool) {
		judgeQuery = `
			select
				judge.id, judge.school school, region.id region, district.id district,
				tab_rating.value tab_rating, chapter.state state
			from (judge, jpool_judge jpj)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
			where judge.id = jpj.judge
				and jpj.jpool = :jpool
				and judge.active = 1
		`;
		replacements.jpool = panel.jpool;
	} else {
		judgeQuery = `
			select
				judge.id, judge.school school, region.id region, district.id district,
				tab_rating.value tab_rating, chapter.state state
			from (judge)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
			where judge.active = 1
				AND (judge.category = :category OR judge.alt_category = :category)
		`;
		replacements.category = panel.category;
	}

	return db.sequelize.query(judgeQuery, {
		replacements,
		type: db.sequelize.QueryTypes.SELECT,
	});
};

export const panelCleanJudges = {

	GET: async (req, res) => {

		const db = req.db;
		const panel = await db.summon(db.panel, req.params.panel_id);

		// First get all the relevant information about the panel we are dealing with
		panel.Settings = await getPanelData(db, req.params.panel_id);
		let cleanJudges = await getPanelJudges(db, panel);

		cleanJudges = await filterBusy(db, panel, cleanJudges);
		cleanJudges = await filterStrikes(db, panel, cleanJudges);
		cleanJudges = await filterSchools(db, panel, cleanJudges);

		res.status(200).json(cleanJudges);
	},
};


export const roundCleanJudges = {
	GET: async (req, res) => {
		console.log(req.params);
		const db = req.db;

		const panel = await db.summon(db.event, req.params.panel_id);

		const settings = await db.query(`
			select
			round.id round, round.type roundType,
			event.id event, event.type eventType,
			category.id category,
			jpool.id jpool

			from (round, event, category)

				left join jpool_round jpr on jpr.round = round.id
				left join jpool on jpr.jpool = jpool.id
				
			where round.id = ${panel.round}
				and round.event = event.id
				and event.category = category.id
		`);

		res.status(200).json(settings);
	},
};
