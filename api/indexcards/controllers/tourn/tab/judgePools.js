export const natsJudgePool = {

	POST: async (req, res) => {
		const db = req.db;
		const tourn = await db.summon(db.tourn, req.params.tourn_id);
		const now = new Date();

		const weights = {
			strikes     : 1000000,
			states      : 100000,
			diversity   : 800,
			diamonds    : 800,
			other_pools : 50,
			remaining   : 50,
			already     : 10,
		};

		if (tourn.start > now) {
			res.status(201).json({
				error   : true,
				message : `Nationals has already started, so I'm gonna go ahead and not let you cause it to explode here.`,
			});
		}

		const parentPool = await db.summon(db.jpool, req.body.parent_id);

		if (!parentPool || !parentPool.settings.registrant ) {
			res.status(201).json({
				error   : true,
				message : `No parent judge pool found for ID ${req.body.parent_id}`,
			});
		}

		if (!req.body.augment) {
			deleteJPoolChildren(db, parentPool.id);
		}

		const jpoolChildren = await getChildPools(db, parentPool.id);
		const judges = await getJPoolJudges(db, parentPool.id, jpoolChildren, weights);

	},
};

export const makeJudgePool = {

	GET: async () => {
		return false;
	},
};

export default makeJudgePool;

const deleteJPoolChildren = async (db, parentId) => {
	// Clear out the old
	await db.sequelize.query(`
		delete jpj.*
			from jpool, jpool_judge jpj
		where jpool.parent = :parentId
			and jpj.jpool = jpool.id
			and jpool.id != jpool.parent

		and not exists (
			select pool_ignore.id
			from jpool_setting pool_ignore
				where pool_ignore.tag = 'pool_ignore'
				and pool_ignore.jpool = jpj.jpool
		)
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.DELETE,
	});
	return true;
};

const getChildPools = async (db, parentId) => {

	const jpoolChildren = await db.sequelize.query(`
		select jpool.id, jpool.name,
			pool_target.value target, pool_priority.value priority,
			rounds.value rounds,
			min(timeslot.start) poolStart,
			max(timeslot.end) poolEnd

		from jpool
			left join jpool_setting pool_target
				on pool_target.tag = 'pool_target'
				and pool_target.jpool = jpool.id

			left join jpool_setting pool_priority
				on pool_priority.tag = 'pool_priority'
				and pool_priority.jpool = jpool.id

			left join jpool_setting rounds
				on rounds.tag = 'rounds'
				and rounds.jpool = jpool.id

			left join jpool_round jpr on jpr.jpool = jpool.id
			left join round on jpr.round = round.id
			left join timeslot on round.timeslot = timeslot.id

		where jpool.parent = :parentId

			and not exists (
				select pool_ignore.id
				from jpool_setting pool_ignore
					where pool_ignore.tag = 'pool_ignore'
					and pool_ignore.jpool = jpool.id
			)

		group by jpool.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	const jpoolsById = {};

	for await (const child of jpoolChildren) {
		child.start = new Date(child.poolStart).valueOf();
		child.end = new Date(child.poolEnd).valueOf();
		jpoolsById[child.id] = child;
	}

	const entryData = await db.sequelize.query(`
		select
			jpool.id jpool, entry.id entry, entry.school, school.region,
			event.type event
		from (entry, event, round, jpool_round jpr, jpool)
			left join school on entry.school = school.id

		where jpool.parent = :parentId
			and jpool.id     = jpr.jpool
			and jpr.round    = round.id
			and round.event  = event.id
			and event.id     = entry.event
			and entry.active = 1
		group by entry.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	for await (const entry of entryData) {

		if (!jpoolsById[entry.jpool].school) {
			jpoolsById[entry.jpool].school = {};
		}
		if (!jpoolsById[entry.jpool].school[entry.school]) {
			jpoolsById[entry.jpool].school[entry.school] = 0;
		}
		if (!jpoolsById[entry.jpool].entries) {
			jpoolsById[entry.jpool].entries = 0;
		}

		jpoolsById[entry.jpool].school[entry.school]++;
		jpoolsById[entry.jpool].entries++;

		if (entry.region) {
			if (!jpoolsById[entry.jpool].state) {
				jpoolsById[entry.jpool].state = {};
			}
			if (!jpoolsById[entry.jpool].state[entry.region]) {
				jpoolsById[entry.jpool].state[entry.region] = 0;
			}
			jpoolsById[entry.jpool].state[entry.region]++;
		}

		if (entry.event === 'congress') {
			jpoolsById.hasCongress = true;
		}
	}

	// Calculate the percentage of each pool that is from each state.  This
	// limitation will be used later to figure out how many judges from that
	// state to put into the pool so we don't ned up with 90% texans or
	// something.

	for await (const jpoolId of Object.keys(jpoolsById)) {
		const jpool = jpoolsById[jpoolId];
		if (jpool.entries) {
			for await (const region of Object.keys(jpool.state)) {
				if (region > 0) {
					jpool.stateShare[jpool.state] = (region / jpool.entries);
				} else {
					jpool.stateShare[jpool.state] = 0;
				}
			}
		}
	}

	return jpoolsById;
};

const getJPoolJudges = async (db, parentId, jpoolChildren, weights) => {

	let limit = '';

	if (!jpoolChildren.hasCongress) {
		limit = `
			and not exists (
				select pref_congress.id
				from judge_setting pref_congress
				where pref_congress.judge = judge.id
				and pref_congress.tag = 'prefers_congress'
			)
		`;
	}
		
	const rawJudges = db.sequelize.query(`
		select
			judge.id, judge.first, judge.last, judge.school, school.region state,
			judge.obligation, judge.hired,
			diverse.value diverse,
			diamonds.value diamonds, 
			nsda_points.value nsda_points,
			count(distinct rpj.jpool) registrant

		from (judge, jpool_judge jpj, jpool, category, tourn)

			left join school on judge.school = school.id
			left join person on judge.person = person.id

			left join judge_setting diverse
				on diverse.tag = 'diverse'
				and diverse.judge = judge.id

			left join person_setting diamonds
				on diamonds.tag = 'diamonds'
				and diamonds.person = person.id

			left join person_setting nsda_points
				on nsda_points.tag = 'nsda_points'
				and nsda_points.person = person.id

			left join jpool_judge rpj 
				on rpj.judge = judge.id
				and exists (
					select jps.id
					from jpool_setting jps
					where jps.jpool = rpj.jpool
					and jps.tag = 'registrant'
				)

		where jpool.id         = :parentId
			and jpool.id       = jpj.jpool
			and jpj.judge      = judge.id
			and judge.active   = 1
			and judge.category = category.id
			and category.tourn = tourn.id
			${limit}
		group by judge.id
	`, {
		replacements : {parentId},
		type         : db.sequelize.QueryTypes.SELECT,
	});

	const judges = {};

	for await (const judge of rawJudges) {

		if (!judges[jid]) { 
			judges[judge.id] = { 
				priority     : 0,
				strikeCount  : 0,
				timeStrikes  : [],
				eventStrikes : {},
				...judge 
			};
			judges[obligation] = parseInt(judge.obligation) + parseInt(judge.hired);
		}

		if (judge.diamonds) {
			judges[judge.id].priority += parseInt(judge.diamonds) * weights.diamonds;
		}

		if (judge.diverse) {
			judges[judge.id].priority += weights.diversity;
		}

		if (judge.obligation > 0) {
			judges[judge.id].priority += parseInt(judge.obligation) * weights.remaining;
		}

		judges[judge.id].priority -= parseInt(judge.registrant) * weights.other_pools; 
	}

	const rawStrikes =  await db.sequelize.query(`
		select
			judge.id judge, 
			strike.id strike, strike.type type,
			strike.start,
			strike.end,
			strike.event,
			tourn.end tournEnd
		from (judge, jpool_judge jpj, jpool, category, tourn)

		where jpool.id         = :parentId
			and jpool.id       = jpj.jpool
			and jpj.judge      = judge.id
			and judge.active   = 1
			and judge.category = category.id
			and category.tourn = tourn.id
			and judge.id       = strike.judge
			and strike.type IN ('event', 'time', 'departure')
	`, {
		replacements : {parentId},
		type         : db.sequelize.QueryTypes.SELECT,
	});

	for await (const strike of rawStrikes) {
		judges[strike.judge].strikeCount++;
		if (strike.type === 'time') {
			judges[strike.judge].timeStrikes.push({
				start : new Date(strike.start).valueOf(),
				end   : new Date(strike.end).valueOf(),
			});
		}

		if (strike.type === 'departure') {
			judges[strike.judge].timeStrikes.push({
				start : new Date(strike.start).valueOf(),
				end   : new Date(strike.tournEnd).valueOf(),
			});
		}

		if (strike.type === 'event') {
			judges[strike.judge].eventStrikes[strike.event] = true;
		}
	}

	const rawJPoolJudges = await db.sequelize.query(`
		select
			judge.id judge,
			jpool.id jpool, jpool.name, rounds.value rounds,
			min(CONVERT_TZ(timeslot.start, '+00:00', tourn.tz)) start,
			max(CONVERT_TZ(timeslot.end, '+00:00', tourn.tz)) end

		from (judge, jpool_judge jpj, jpool, jpool_judge rpj, category, tourn)

			left join jpool_round jpr on jpr.jpool = jpool.id
			left join round on jpr.round = round.id
			left join timeslot on timeslot.id = round.timeslot

			left join jpool_setting rounds
				on rounds.tag = 'rounds'
				and rounds.jpool = jpool.id

		where rpj.jpool = :parentId
			and rpj.judge = judge.id
			and judge.id = jpj.judge
			and jpj.jpool = jpool.id
			and jpj.jpool != rpj.jpool
			and jpool.category = category.id
			and category.tourn = tourn.id
			${limit}
		group by jpj.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	while (
		my (
			$jid, $jpool_id, $jpool_name,
			$rounds,
			$timeslot_start, $timeslot_end
		) = $jpool_sth->fetchrow_array()
	) {

		unless ($judges{$jid}{"already"}{$jpool_id}) {

			if ($rounds) {
				$judges{$jid}{"obligation"} -= $rounds;
				$judges{$jid}{"priority"} -= $scores{"remaining"} * $rounds;
			}

			$judges{$jid}{"priority"} += $rounds * $scores{"already"};

			unless (defined $dt_cache{$timeslot_start}) {

				$dt_cache{$timeslot_start} = eval {
					my $dt = DateTime::Format::MySQL->parse_datetime($timeslot_start);
					return $dt->epoch;
				};
			}
			unless (defined $dt_cache{$timeslot_end}) {
				$dt_cache{$timeslot_end} = eval {
					my $dt = DateTime::Format::MySQL->parse_datetime($timeslot_end);
					return $dt->epoch;
				};
			}

			foreach my $tid (keys %targets) {
				if (
					$targets{$tid}{"epoch_start"} <= $dt_cache{$timeslot_end}
					&& $targets{$tid}{"epoch_end"} >= $dt_cache{$timeslot_start}
				) {
					$judges{$jid}{"exclude"}{$tid}++;
				}
			}
		}
	}
};
