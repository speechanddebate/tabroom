// import { showDateTime } from '../../../helpers/common';

export const panelCleanJudges = {

	GET: async (req, res) => {

		const db = req.db;
		const rawPanel = await db.summon(db.panel, req.params.panel_id);

		// First get all the relevant information about the panel we are dealing with
		rawPanel.Entries = await getPanelEntries(db, rawPanel);

		// Trade out for an actually complete representation of the panel.
		const panel = await getPanelData(db, rawPanel);

		let cleanJudges = await getPanelJudges(db, panel);
		cleanJudges = await filterBusy(db, panel, cleanJudges);
		cleanJudges = await filterStrikes(db, panel, cleanJudges);
		cleanJudges = await filterSchools(db, panel, cleanJudges);

		res.status(200).json(cleanJudges);
	},
};

const getPanelData = async (db, rawPanel) => {

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
			tab_ratings.value tabRatings,
			timeslot.start start,
			timeslot.end end

		from (round, event, category, timeslot)

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
			and round.timeslot = timeslot.id
	`, {
		replacements: { panel: rawPanel.id },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const panel = { ...rawPanel, ...settings };
	panel.EntrySchools = {};
	panel.EntryRegions = {};
	panel.EntryDistricts = {};
	panel.EntryStates = {};
	panel.Entries = {};

	rawPanel.Entries.forEach( (entry) => {

		panel.Entries[entry.id] = entry;

		if (entry.school) {
			panel.EntrySchools[entry.school] = true;
		}
		if (entry.hybrid) {
			panel.EntrySchools[entry.hybrid] = true;
		}
		if (entry.region) {
			panel.EntryRegions[entry.region] = true;
		}
		if (entry.district) {
			panel.EntryDistricts[entry.district] = true;
		}
		if (entry.state) {
			panel.EntryStates[entry.state] = true;
		}
	});

	return panel;
};

const getPanelEntries = async (db, panel) => {

	const entryQuery = `
		select
			entry.id,
			entry.school,
			region.id region,
			district.id district,
			chapter.state state,
			hybrid.school hybrid
		from (ballot, entry)

			left join school on entry.school = school.id
			left join region on school.region = region.id
			left join district on school.district = district.id
			left join chapter on school.chapter = chapter.state
			left join strike hybrid on hybrid.type = 'hybrid' and hybrid.entry = entry.id

		where ballot.panel = :panelId
			and ballot.entry = entry.id
			and entry.active = 1
	`;

	return db.sequelize.query(entryQuery, {
		replacements: { panelId: panel.id },
		type: db.sequelize.QueryTypes.SELECT,
	});
};

const getPanelJudges = async (db, panel) => {

	let judgeQuery = '';
	const replacements = {};

	if (panel.jpool) {
		judgeQuery = `
			select
				judge.id, judge.person, judge.school school, region.id region, district.id district,
				tab_rating.value tab_rating, chapter.state state, neutral.value value
			from (judge, jpool_judge jpj)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
				left join judge_setting neutral on neutral.tag = 'neutral' and neutral.judge = judge.id
			where judge.id = jpj.judge
				and jpj.jpool = :jpool
				and judge.active = 1
		`;
		replacements.jpool = panel.jpool;
	} else {
		judgeQuery = `
			select
				judge.id, judge.school school, region.id region, district.id district,
				tab_rating.value tab_rating, chapter.state state, neutral.value value
			from (judge)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
				left join judge_setting neutral on neutral.tag = 'neutral' and neutral.judge = judge.id
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

const filterBusy = async (db, panel, cleanJudges) => {
	const busyJudges = await db.sequelize.query(`
		select ballot.judge, judge.person
		from ballot, panel, round, timeslot, judge
		where timeslot.start < :end
			and timeslot.end < :start
			and timeslot.tourn = :tourn
			and timeslot.id = round.timeslot
			and round.id = panel.round
			and panel.id = ballot.panel
			and ballot.judge = judge.id
		group by judge.id
	`);

	const iAmBusy = {};

	busyJudges.forEach( (judge) => {
		if (judge.person) {
			iAmBusy.people[judge.person] = true;
		}
		iAmBusy.judges[judge.judge] = true;
	});

	const stillCleanJudges = [];

	cleanJudges.forEach( (judge) => {
		if (!iAmBusy.judges[judge.id] && !(judge.person && iAmBusy.people[judge.person])) {
			stillCleanJudges.push(judge);
		}
	});
	return stillCleanJudges;
};

const filterStrikes = async (db, panel, cleanJudges) => {

	const judgeStrikes = await db.sequelize.query(`
		select strike.*
		from strike, judge, category
		where category.tourn = ?
			and category.id = judge.category
			and judge.active = 1
			and judge.strike = strike.id
	`);

	const struckJudges = {};

	judgeStrikes.forEach( (strike) => {

		if (strike.type === 'time') {

			// OK if the strike ends before I start, or starts after I end
			if (panel.start > strike.end
				|| panel.end < strike.start
			) {
				return;
			}

		} else if (strike.type === 'departure') {

			// OK if the round is over before the departure time.
			if (panel.end < strike.start) {
				return;
			}

		} else if (strike.type === 'elim') {

			// Elim-only reserved judge.  OK if we're in elims or not this event.
			if ( panel.event != strike.event
				|| panel.roundType === 'elim'
				|| panel.roundType === 'final'
				|| panel.roundType === 'runoff'
			) {
				return;
			}
		} else if (strike.type === 'event') {

			// OK if it's not this event
			if (panel.event != strike.event) {
				return;
			}

		} else if (strike.type === 'school') {

			// That school is not present in the section
			if (!panel.EntrySchools[strike.school]) {
				return;
			}

		} else if (strike.type === 'region') {

			// That school is not present in the section
			if (!panel.EntrySchools[strike.school]) {
				return;
			}

		} else if (strike.type === 'state') {

			if (!panel.EntryStates[strike.state]) {
				return;
			}

		} else if (strike.type === 'district') {

			if (!panel.EntryDistricts[strike.district]) {
				return;
			}

		} else if (strike.type === 'entry') {

			if (!panel.Entries[strike.entry]) {
				return;
			}
		}
		struckJudges[strike.judge] = true;
	});

	const stillCleanJudges = [];

	cleanJudges.forEach( (judge) => {

		if (struckJudges[judge.id]) {
			return;
		}
		stillCleanJudges.push(judge);
	});

	return stillCleanJudges;
};

export const roundCleanJudges = {
	GET: async (req, res) => {
		console.log(req.params);
		const db = req.db;

		const panel = await db.summon(db.event, req.params.panel_id);

		res.status(200).json(settings);
	},
};
