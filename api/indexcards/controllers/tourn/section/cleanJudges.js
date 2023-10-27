// import { showDateTime } from '../../../helpers/common';

export const panelCleanJudges = {

	GET: async (req, res) => {

		const db = req.db;
		const panel = await db.summon(db.panel, req.params.sectionId);

		// Pull settings and everything else we need about this round
		panel.round = await roundData(db, panel.round);

		// Get the information and relevant data about the entries in my panel
		panel.entries = await panelEntries(db, panel);

		// Pull the judges who are available to judge this round timewise
		panel.round.judges = await roundAvailableJudges(db, panel.round);

		// Pull the entry constraints against juges
		const judgeConflicts = await roundJudgeConflicts(db, panel.round);

		const cleanJudges = panel.round.judges.filter( (judge) => {
			if (judgeConflicts[judge.id]
				&& panel.entries.Entries.some( entry => judgeConflicts[judge.id].indexOf(entry.id) !== -1)
			) {
				return false;
			}
			return judge;
		});

		res.status(200).json(cleanJudges);
	},
};

const roundData = async (db, roundId) => {

	const [round] = await db.sequelize.query(`
		select
			round.id id, round.type type, round.timeslot timeslot,
			event.id event, event.type eventType,
			category.id category,
			jpool.id jpool,
			neutrals.value neutrals,
			auto_conflict_hires.value auto_conflict_hires,
			no_first_years.value no_first_years,
			allow_school_panels.value allow_school_panels,
			allow_region_panels.value allow_region_panels,
			region_judge_forbid.value region_judge_forbidid,
			conflict_dioregion_judges.value conflict_dioregion_judges,
			diocese_regions.value_text diocese_regions,
			allow_repeat_judging.value allow_repeat_judging,
			allow_repeat_elims.value allow_repeat_elims,
			allow_repeat_prelim_side.value allow_repeat_prelim_side,
			disallow_repeat_drop.value disallow_repeat_drop,
			online_mode.value online_mode,
			dumb_half_async_thing.value dumb_async,
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

			left join category_setting allow_school_panels
				on allow_school_panels.category = category.id
				and allow_school_panels.tag = "allow_school_panels"

			left join category_setting allow_region_panels
				on allow_region_panels.category = category.id
				and allow_region_panels.tag = "allow_region_panels"

			left join event_setting no_first_years
				on no_first_years.event = event.id
				and no_first_years.tag = "no_first_years"

			left join event_setting region_judge_forbid
				on region_judge_forbid.event = event.id
				and region_judge_forbid.tag = "region_judge_forbid"

			left join event_setting conflict_dioregion_judges
				on conflict_dioregion_judges.event = event.id
				and conflict_dioregion_judges.tag = "conflict_dioregion_judges"

			left join event_setting diocese_regions
				on diocese_regions.event = event.id
				and diocese_regions.tag = "diocese_regions"

			left join event_setting allow_repeat_judging
				on allow_repeat_judging.event = event.id
				and allow_repeat_judging.tag = "allow_repeat_judging"

			left join event_setting allow_repeat_elims
				on allow_repeat_elims.event = event.id
				and allow_repeat_elims.tag = "allow_repeat_elims"

			left join event_setting allow_repeat_prelim_side
				on allow_repeat_prelim_side.event = event.id
				and allow_repeat_prelim_side.tag = "allow_repeat_prelim_side"

			left join event_setting disallow_repeat_drop
				on disallow_repeat_drop.event = event.id
				and disallow_repeat_drop.tag = "disallow_repeat_drop"

			left join event_setting online_mode
				on online_mode.event = event.id
				and online_mode.tag = "online_mode"

			left join event_setting dumb_half_async_thing
				on dumb_half_async_thing.event = event.id
				and dumb_half_async_thing.tag = "dumb_half_async_thing"

			left join jpool_round jpr on jpr.round = round.id
			left join jpool on jpr.jpool = jpool.id

		where round.id = :roundId
			and round.event = event.id
			and event.category = category.id
			and round.timeslot = timeslot.id
	`, {
		replacements: { roundId },
		type: db.sequelize.QueryTypes.SELECT,
	});

	if (round.conflict_dioregion_judges && round.diocese_regions) {
		round.dioregions = JSON.parse(round.diocese_regions);
	}

	if (round.type === 'final' || round.type === 'runoff') {
		round.type = 'elim';
	} else {
		round.type = 'prelim';
	}

	return round;
};

const panelEntries = async (db, panel) => {

	const entryQuery = `
		select
			entry.id,
			entry.school,
			region.id region,
			district.id district,
			chapter.state state,
			hybrid.school hybrid,
			ballot.side side
		from (ballot, entry)

			left join school on entry.school = school.id
			left join region on school.region = region.id
			left join district on school.district = district.id
			left join chapter on school.chapter = chapter.state
			left join strike hybrid on hybrid.type = 'hybrid' and hybrid.entry = entry.id

		where ballot.panel = :sectionId
			and ballot.entry = entry.id
			and entry.active = 1
	`;

	const rawEntries = await db.sequelize.query(entryQuery, {
		replacements: { sectionId: panel.id },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const entries = {};
	entries.Entries = [];
	entries.EntrySchools = {};
	entries.EntryRegions = {};
	entries.EntrydioRegions = {};
	entries.EntryDistricts = {};
	entries.EntryStates = {};

	rawEntries.forEach(  (entry) => {

		entries.Entries.push(entry);

		if (entry.school) {
			entries.EntrySchools[entry.school] = true;
		}
		if (entry.hybrid) {
			entries.EntrySchools[entry.hybrid] = true;
		}
		if (entry.region) {
			entries.EntryRegions[entry.region] = true;
		}
		if (entry.district) {
			entries.EntryDistricts[entry.district] = true;
		}
		if (entry.state) {
			entries.EntryStates[entry.state] = true;
		}
	});

	return entries;
};

const roundAvailableJudges = async (db, round) => {

	// Returns a list of judges who can judge this round, filtering out any
	// judge currently judging a non-async round, with a time constraint, or
	// those blocked against the event.

	// This should be the only time I actually resort to the adaptive sql
	// nonsense approach that the predecessor of this code did most foully, but
	// in this case it truly is the most efficient way to do things, both in
	// terms of code density and execution speed. I'm sorry.  I really am.
	// -- CLP

	let judgeQuery = `
		select
			judge.id, judge.first, judge.middle, judge.last, judge.code,
			judge.hired, judge.obligation,
			judge.person, judge.school school, region.id region, district.id district,
			tab_rating.value tab_rating, chapter.state state, neutral.value neutral
	`;

	if (round.jpool) {
		// Pull judges from the judge pools linked to this round
		judgeQuery = ` ${judgeQuery}
			from (judge, jpool_judge jpj, jpool_round jpr, round, timeslot)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
				left join judge_setting neutral on neutral.tag = 'neutral' and neutral.judge = judge.id
			where jpr.round = :roundId
				and jpr.jpool = jpj.jpool
				and jpj.judge = judge.id
				and judge.active = 1
				and jpr.round = round.id
				and round.timeslot = timeslot.id
		`;
	} else {

		// Pull judges from the judge category linked to this round
		judgeQuery = ` ${judgeQuery}
			from (judge, round, event, timeslot)
				left join school on judge.school = school.id
				left join region on school.region = region.id
				left join district on school.district = district.id
				left join chapter on school.chapter = chapter.id
				left join judge_setting tab_rating on tab_rating.tag = 'tab_rating' and tab_rating.judge = judge.id
				left join judge_setting neutral on neutral.tag = 'neutral' and neutral.judge = judge.id
			where judge.active = 1
				AND (judge.category = event.category OR judge.alt_category = event.category)
				and event.id = round.event
				AND round.id = :roundId
				and round.timeslot = timeslot.id
		`;
	}

	// No event constraints please.
	judgeQuery = ` ${judgeQuery}
		and not exists (
			select evs.id
				from strike evs
			where evs.event = round.event
				and evs.judge = judge.id
				and evs.type = 'event'
		) `;

	// No elim constrained judges if we're not an elim.
	if (round.type === 'prelim') {
		judgeQuery = ` ${judgeQuery}
			and not exists (
				select els.id
					from strike els
				where els.event = round.event
					and els.type = 'elim'
					and els.judge = judge.id
			) `;
	}

	// No FYOs if we don't allow them
	if (round.no_first_years) {
		judgeQuery = ` ${judgeQuery}
			and not exists (
				select first_year.id
				from judge_setting first_year
				where first_year.tag = 'first_year'
				and first_year.judge = judge.id
			) `;
	}

	if (round.online_mode !== 'async') {
		// No time constraints that cover the present
		judgeQuery = ` ${judgeQuery}
			and not exists (
				select strike.id
					from strike
				where strike.type IN ('time', 'departure')
					and strike.start < timeslot.end
					and strike.end > timeslot.start
					and strike.judge = judge.id
			)
		`;
	}

	const initialJudges = await db.sequelize.query(judgeQuery, {
		replacements: { roundId: round.id },
		type: db.sequelize.QueryTypes.SELECT,
	});

	let busyQuery = `
		select
			judge.id, judge.person
		from judge, ballot, panel, round, timeslot, timeslot t2

			where judge.id = ballot.judge
				and ballot.panel = panel.id
				and panel.round = round.id
				and round.timeslot = timeslot.id
				and timeslot.tourn = t2.tourn
				and t2.id = :timeslotId
				and not exists (
					select es.id
					from event_setting es
					where es.event = round.event
					and es.tag = 'online_mode'
					and es.value = 'async'
				)
	`;

	if (round.no_back_to_back) {
		busyQuery = ` ${busyQuery}
			and t2.start <= timeslot.end
			and t2.end >= timeslot.start
		`;
	} else {
		busyQuery = ` ${busyQuery}
			and t2.start < timeslot.end
			and t2.end > timeslot.start
		`;
	}

	const busyFolks = await db.sequelize.query(busyQuery, {
		replacements: { timeslotId: round.timeslot },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const busyJudges = {};
	const busyPeople = {};

	busyFolks.forEach( (folk) => {
		if (folk.judge) {
			busyJudges[folk.judge] = true;
		}
		if (folk.person) {
			busyPeople[folk.person] = true;
		}
	});

	const judges = [];

	initialJudges.forEach( (judge) => {
		if (busyJudges[judge.id]) {
			return;
		}
		if (judge.person && busyPeople[judge.person]) {
			return;
		}
		judges.push(judge);
	});

	return judges;
};

export const roundJudges =  {
	GET: async (req, res) => {
		const round = await roundData(req.db, req.params.roundId);
		const availableJudges = await roundAvailableJudges(req.db, round);
		res.status(200).json(availableJudges);
	},
};

const roundJudgeConflicts = async (db, round) => {

	const judgeConflicts = {};

	const roundEntries = await db.sequelize.query(`
		select
			entry.id, school.id school, region.id region, district.id district, hybrid.school hybrid, ballot.side side
		from (entry, ballot, panel)
			left join school on entry.school = school.id
			left join region on school.region = region.id
			left join district on school.district = district.id
			left join strike hybrid on hybrid.type = 'hybrid' and hybrid.entry = entry.id
		where entry.id = ballot.entry
			and ballot.panel = panel.id
			and panel.round = :roundId
	`, {
		replacements: { roundId: round.id },
		type: db.sequelize.QueryTypes.SELECT,
	});

	const entriesBy    = {};

	entriesBy.school   = {};
	entriesBy.region   = {};
	entriesBy.dioregion   = {};
	entriesBy.district = {};
	entriesBy.side     = {};

	roundEntries.forEach( (entry) => {

		if (entry.school) {
			entriesBy.school[entry.school] = [entry.id, ...entriesBy.school[entry.school] ?? []];
		}

		if (entry.hybrid) {
			entriesBy.school[entry.hybrid] = [entry.id, ...entriesBy.school[entry.hybrid] ?? []];
		}

		if (round.conflict_dioregion_judges && round.dioregions[entry.region]) {
			entry.dioregion = round.dioregions[entry.region];
			entriesBy.dioregion[entry.dioregion] = [
				entry.id,
				...entriesBy.dioregion[entry.dioregion] ?? [],
			];
		}

		if (entry.region) {
			entriesBy.region[entry.region] = [entry.id, ...entriesBy.region[entry.region] ?? []];
		}

		if (entry.district) {
			entriesBy.district[entry.district] = [entry.id, ...entriesBy.district[entry.district] ?? []];
		}

		if (entry.side) {
			entriesBy.side[entry.id] = entry.side;
		}
	});

	if (!round.allow_repeat_judging) {
		const ballotConflicts = await db.sequelize.query(`
			select
				entry.id entry, ballot.judge, ballot.side, winloss.id winloss, winloss.value winner
			from (entry, ballot, panel, round)
				left join score winloss on winloss.tag = 'winloss' and winloss.ballot = ballot.id
			where exists (
					select b1.id
					from ballot b1, panel p1
					where b1.entry = entry.id
					and b1.panel = p1.id
					and p1.round = :roundId
				)
				and entry.id = ballot.entry
				and ballot.panel = panel.id
				and panel.round != :roundId
				and panel.round = round.id
				and ballot.judge > 0
		`, {
			replacements: { roundId: round.id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		ballotConflicts.forEach( (ballot) => {

			if (!judgeConflicts[ballot.judge]) {
				judgeConflicts[ballot.judge] = [];
			}

			if (round.type === 'elim') {
				if (round.allow_repeat_elims) {
					if (!round.disallow_repeat_drop
						|| ballot.winner
					) {
						// No conflict because either I won or it doesn't matter
						return;
					}
				}
				judgeConflicts[ballot.judge] = [ballot.entry, ...judgeConflicts[ballot.judge] ?? []];
			} else {
				if (round.allow_repeat_prelim_side && entriesBy.side[ballot.entry] === ballot.side) {
					judgeConflicts[ballot.judge].push(ballot.entry);
				} else if (!round.allow_repeat_prelim_side) {
					judgeConflicts[ballot.judge].push(ballot.entry);
				}
			}
		});
	}

	// Process any entry, school, region or district strikes against the judges available
	let judgeStrikesQuery = `
		select
			judge.id judge, strike.type, strike.entry, strike.school, strike.district, strike.region
	`;

	if (round.jpool) {
		// Pull judges from the judge pools linked to this round
		judgeStrikesQuery = ` ${judgeStrikesQuery}
			from (judge, jpool_judge jpj, jpool_round jpr, strike)
			where judge.id = strike.judge
				and jpj.judge = judge.id
				and jpr.round = :roundId
				and jpr.jpool = jpj.jpool
				and judge.active = 1
		`;
	} else {
		// Pull judges from the judge category linked to this round
		judgeStrikesQuery = ` ${judgeStrikesQuery}
			from (judge, strike, round, event)
			where judge.id = strike.judge
				and round.id = :roundId
				and round.event = event.id
				and event.category = judge.category
				and judge.active = 1
		`;
	}

	const judgeStrikes = await db.sequelize.query(
		judgeStrikesQuery,{
			replacements: { roundId: round.id },
			type: db.sequelize.QueryTypes.SELECT,
		});

	judgeStrikes.forEach( (strike) => {

		if (!judgeConflicts[strike.judge]) {
			judgeConflicts[strike.judge] = [];
		}

		if (strike.type === 'school' && entriesBy.school[strike.school]) {
			judgeConflicts[strike.judge] = [
				...entriesBy.school[strike.school],
				...judgeConflicts[strike.judge] ?? [],
			];

		} else if (strike.type === 'region' && entriesBy.region[strike.region]) {
			judgeConflicts[strike.judge] = [
				...entriesBy.region[strike.region],
				...judgeConflicts[strike.judge] ?? [],
			];

		} else if (strike.type === 'district' && entriesBy.district[strike.district]) {
			judgeConflicts[strike.judge] = [
				...entriesBy.district[strike.district],
				...judgeConflicts[strike.judge] ?? [],
			];
		} else if (strike.type === 'entry') {
			judgeConflicts[strike.judge] = [
				strike.entry,
				...judgeConflicts[strike.judge] ?? [],
			];
		}
	});

	if (round.auto_conflict_hires) {

		const judgeHires = await db.sequelize.query(`
			select judge_hire.judge, judge_hire.school
				from (judge_hire, school, entry, ballot, panel)
			where panel.round = :roundId
				and panel.id = ballot.panel
				and ballot.entry = entry.id
				and entry.school = school.id
				and school.id = judge_hire.school
			group by judge_hire.id
		`, {
			replacements: { roundId: round.id },
			type: db.sequelize.QueryTypes.SELECT,
		});

		judgeHires.forEach( (hire) => {
			if (!judgeConflicts[hire.judge]) {
				judgeConflicts[hire.judge] = [];
			}

			if (hire.school && entriesBy.school[hire.school]) {
				judgeConflicts[hire.judge] = [
					...entriesBy.school[hire.school],
					...judgeConflicts[hire.judge] ?? [],
				];
			}
		});
	}

	if (!round.allow_judge_own) {

		// Conflict any entries from one's own school if that's required.

		round.judges.forEach( (judge) => {

			if (judge.school && entriesBy.school[judge.school]) {
				judgeConflicts[judge.id] = [
					...entriesBy.school[judge.school],
					...judgeConflicts[judge.id] ?? [],
				];
			}

			if (round.region_judge_forbid && judge.school && entriesBy.school[judge.school]) {
				judgeConflicts[judge.id] = [
					...entriesBy.region[judge.region],
					...judgeConflicts[judge.id] ?? [],
				];
			}

			if (round.conflict_dioregion_judges && round.dioregions[judge.region]) {
				judgeConflicts[judge.id] = [
					...entriesBy.dioregion[judge.dioregion],
					...judgeConflicts[judge.id] ?? [],
				];
			}
		});
	}

	return judgeConflicts;
};
