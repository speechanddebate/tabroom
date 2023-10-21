export const natsJudgePool = {

	POST: async (req, res) => {

		const db = req.db;
		const tourn = await db.summon(db.tourn, req.params.tourn_id);
		const now = new Date();

		const weights = {
			strikes    : 10000000,
			states     : 1000000,
			preference : 10000,
			diversity  : 1000,
			diamonds   : 800,
			otherPools : 500,
			remaining  : 200,
			schoolSize : 25,
			already    : 10,
		};

		if (tourn.start < now) {
			res.status(201).json({
				error   : true,
				message : `Nationals has already started, so I'm gonna go ahead and not let you lob a grenade into it here.`,
			});
			return;
		}

		const parentPool = await db.summon(db.jpool, req.body.parent_id);

		if (!parentPool) {
			res.status(201).json({
				error   : true,
				message : `No parent judge pool found for ID ${req.body.parent_id}`,
			});
			return;
		}

		if (!req.body.augment) {
			deleteJPoolChildren(db, parentPool.id);
		}

		const children = await getChildPools(db, parentPool.id);
		const judges = await getJPoolJudges(db, parentPool.id, children, weights);
		const jpoolsByPriority = {};

		// Now arrange the pools into arrays keyed by the priority.  Do this so
		// that pools at the same priority tier are each round-robin assigned
		// judging, so that the very last pool doesn't end up with zero
		// diversity, diamonds or state balance.

		Object.keys(children).forEach( (jpoolId) => {

			const jpool = children[jpoolId];

			if (!jpool.priority) {
				// this ugly hack is in homage to Dr Jon Bruschke
				jpool.priority = 999;
			}
			if (!jpoolsByPriority[jpool.priority]) {
				jpoolsByPriority[jpool.priority] = [];
			}
			jpoolsByPriority[jpool.priority].push(jpool);
		});

		const jpoolJudges = {};
		let counter = 0;

		Object.keys(jpoolsByPriority).sort( ).forEach( (priority) => {

			const jpools = jpoolsByPriority[priority];
			const schoolPoolCount = {
				total : {},
				done  : {},
			};

			jpools.forEach( (jpool) => {
				schoolPoolCount[jpool.id] = {};

				if (!jpool.schoolCount) {
					return;
				}

				Object.keys(jpool.schoolCount).forEach( (schoolId) => {
					schoolPoolCount[jpool.id][schoolId] = jpool.schoolCount[schoolId];
					if (!schoolPoolCount.total[schoolId]) {
						schoolPoolCount.total[schoolId] = jpool.schoolCount[schoolId];
					} else {
						schoolPoolCount.total[schoolId] += jpool.schoolCount[schoolId];
					}
				});
			});

			const schoolPercent = {};

			Object.keys(judges).forEach( (judgeId)  => {

				if (!judges[judgeId]) { return; }
				const judge = judges[judgeId];

				if (judge.school
					&& schoolPoolCount.total[judge.school]
					&& !schoolPoolCount.done[judge.school]
				) {
					jpools.forEach( (jpool) => {

						if (!schoolPercent[jpool.id]) {
							schoolPercent[jpool.id] = {};
						}

						if (schoolPoolCount[jpool.id]?.[judge.school]) {
							schoolPercent[jpool.id][judge.school] = (
								schoolPoolCount[jpool.id][judge.school]
								/ schoolPoolCount.total[judge.school]);
						} else {
							schoolPercent[jpool.id][judge.school] = 0;
						}
					});
					schoolPoolCount.done[judge.school] = true;
				}
			});

			while (jpools.length > 0 && Object.keys(judges).length > 1) {

				const jpool = jpools.shift();

				if (!jpoolJudges[jpool.id]) {
					jpoolJudges[jpool.id] = [];
				}

				const judgeKeys = Object.keys(judges).sort( (a, b) => {

					if (priority < 4 && (judges[a].priority !== judges[b].priority)) {
						return judges[b].priority - judges[a].priority;
					}

					if (jpool.site_preference) {
						if (judges[a].site_preference !== jpool.site_preference
							&& judges[b].site_preference === jpool.site_preference
						) {
							return 1000;
						}

						if (judges[b].site_preference !== jpool.site_preference
							&& judges[a].site_preference === jpool.site_preference
						) {
							return -1000;
						}
					}

					if (schoolPercent[jpool.id]?.[judges[a]?.school]
						!== schoolPercent[jpool.id]?.[judges[b]?.school]
					) {
						return (schoolPercent[jpool.id][judges[b].school]
							- schoolPercent[jpool.id][judges[a].school]);
					}
					return judges[b].priority - judges[a].priority;
				});

				let chosenOne = 0;
				judgeKeys.forEach( (judgeId) => {

					if (!judges[judgeId]) {
						return;
					}
					const judge = judges[judgeId];

					// Keep Congress judges in the SAME event for the two
					// prelims and DIFFERENT events for elims

					if (jpool.eventType === 'congress') {

						if (jpool.roundType === 'prelim') {
							if (judge.alreadyPrelim && judge.alreadyPrelim !== jpool.eventAbbr) {
								return;
							}

							if (judge.alreadyPrelim !== jpool.eventAbbr
								&& judge.alreadyCongress[jpool.eventAbbr]
							) {
								return;
							}

						} else {
							if (judge.alreadyCongress[jpool.eventAbbr]) {
								return;
							}
						}
					}

					if (schoolPoolCount.total[judge.school]
						&& !schoolPercent[jpool.id][judge.school]
						&& !req.body.augment
						&& jpool.eventType !== 'congress'
					) {
						return;
					}

					if (judge.rounds < jpool.rounds
						|| judge.exclude?.[jpool.id]
						|| chosenOne > 0
					) {
						return;
					}

					// Tag the search as successfull, and reduce the target
					// count for this pool
					chosenOne = judgeId;
					jpool.target--;

					// Put them into this pool
					if (!jpoolJudges[jpool.id]) {
						jpoolJudges[jpool.id] = [judgeId];
					} else {
						jpoolJudges[jpool.id].push(judgeId);
					}

					// Adjust the round obligation downwards to match this guy
					judge.rounds -= jpool.rounds;

					// prevent Congress from getting all screwed up
					if (jpool.eventType === 'congress') {
						judge.alreadyCongress[jpool.eventAbbr] = true;
						if (jpool.roundType === 'prelim') {
							judge.alreadyPrelim = jpool.eventAbbr;
						}
					}

					// If the judge is out of obligation then remove them from
					// the potentials pile

					if (judge.rounds < 1) {
						delete judges[judge.id];
					} else {

						// Exclude them from other jpools that overlap mine in time
						Object.keys(children).forEach( (childId) => {
							if (jpool.start <= children[childId].end && jpool.end >= children[childId].start) {
								judge.exclude[childId] = true;
							}
						});

						// Nuke the coverage percentages to reflect that the kids here
						// are already chaperoned

						if (schoolPercent[jpool.id]?.[judge.school]) {
							// Dividing by 100 means that all kids will get at
							// least 1 chaperone, but judges will still be
							// preferred for sites where they have any kids at all.
							schoolPercent[jpool.id][judge.school] /= 100;
						}
					}
				});

				counter++;

				// Unless I've hit quota I get another go around, unless I was
				// not able to find any judges at all, in which case break the
				// loop.

				if (chosenOne && jpool.target > 0) {
					jpools.push(jpool);
				}
			}
		});

		// Write them in
		Object.keys(jpoolJudges).forEach( (jpoolId) => {
			jpoolJudges[jpoolId].forEach( async (judgeId) => {
				await db.sequelize.query(
					`insert into jpool_judge (judge, jpool) values (:judgeId, :jpoolId)`,
					{
						replacements : { judgeId, jpoolId },
						type         : db.sequelize.QueryTypes.INSERT,
					}
				);
			});
		});

		const message = `${Object.keys(jpoolJudges).length} pools populated with ${counter} assignments`;
		res.status(200).json({
			error: false,
			message,
			redirect: `/panel/judge/pool_report.mhtml?parent_id=${parentPool.id}msg=${message}`,
		});
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
			max(timeslot.end) poolEnd,
			event.type eventType,
			event.abbr eventAbbr,
			round.type roundType,
			site_preference.value site_preference

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

			left join jpool_setting site_preference
				on site_preference.tag = 'site_preference'
				and site_preference.jpool = jpool.id

			left join jpool_round jpr on jpr.jpool = jpool.id
			left join round on jpr.round = round.id
			left join event on round.event = event.id
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

	for (const child of jpoolChildren) {
		child.start = new Date(child.poolStart).valueOf();
		child.end = new Date(child.poolEnd).valueOf();
		jpoolsById[child.id] = child;
	}

	const entryData = await db.sequelize.query(`
		select
			jpool.id jpool, jpool.name jpoolname,
			entry.id entry, entry.school, entry.code,
			school.region,
			event.type event
		from (entry, event, round, jpool_round jpr, jpool)
			left join school on entry.school = school.id

		where jpool.parent        = :parentId
			and jpool.id          = jpr.jpool
			and jpr.round         = round.id
			and round.event       = event.id
			and event.id          = entry.event
			and entry.unconfirmed = 0
			and not exists (
				select pool_ignore.id
				from jpool_setting pool_ignore
					where pool_ignore.tag = 'pool_ignore'
					and pool_ignore.jpool = jpool.id
			)
		group by entry.id, jpool.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	for (const entry of entryData) {

		if (!jpoolsById[entry.jpool].schoolCount) {
			jpoolsById[entry.jpool].schoolCount = {};
		}
		if (!jpoolsById[entry.jpool].schoolCount[entry.school]) {
			jpoolsById[entry.jpool].schoolCount[entry.school] = 0;
		}
		if (!jpoolsById[entry.jpool].entries) {
			jpoolsById[entry.jpool].entries = 0;
		}

		jpoolsById[entry.jpool].schoolCount[entry.school]++;
		jpoolsById[entry.jpool].entries++;

		if (!jpoolsById[entry.jpool].state) {
			jpoolsById[entry.jpool].state = {};
		}

		if (entry.region) {
			if (!jpoolsById[entry.jpool].state[entry.region]) {
				jpoolsById[entry.jpool].state[entry.region] = 0;
			}
			jpoolsById[entry.jpool].state[entry.region]++;
		}
	}

	// Calculate the percentage of each pool that is from each state.  This
	// limitation will be used later to figure out how many judges from that
	// state to put into the pool so we don't ned up with 90% texans or
	// something.

	for (const jpoolId of Object.keys(jpoolsById)) {
		const jpool = jpoolsById[jpoolId];
		if (jpool.entries) {
			for (const region of Object.keys(jpool.state)) {
				jpool.stateShare = {};
				if (region > 0) {
					jpool.stateShare[jpool.state] = (jpool.state[region] / jpool.entries);
				} else {
					jpool.stateShare[jpool.state] = 0;
				}
			}
		}
	}
	return jpoolsById;
};

const getJPoolJudges = async (db, parentId, jpools, weights) => {

	const rawJudges = await db.sequelize.query(`
		select
			judge.id, judge.first, judge.last, judge.school, school.region state,
			judge.obligation, judge.hired,
			diverse.value diverse,
			site_preference.value site_preference,
			diamonds.value diamonds,
			nsda_points.value nsda_points,
			count(distinct rpj.jpool) registrant,
			count(distinct entry.id) entries

		from (judge, jpool_judge jpj, jpool, category, tourn)

			left join school on judge.school = school.id
			left join entry on school.id = entry.school and entry.unconfirmed = 0

			left join person on judge.person = person.id

			left join judge_setting site_preference
				on site_preference.tag = 'site_preference'
				and site_preference.judge = judge.id

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
		group by judge.id
		order by judge.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	const prioritize = (item) => {
		let priority = 0;

		priority += parseInt(item.diamonds ? item.diamonds : 0) * weights.diamonds;
		priority += parseInt(item.diverse ? 1 : 0) * weights.diversity;
		priority += parseInt(item.obligation ? item.obligation : 0) * weights.remaining;
		priority += parseInt(item.hired ? item.hired : 0) * weights.remaining;
		priority += parseInt(item.registrant ? item.registrant : 0) * weights.otherPools;
		// These things should make you less important (because you fit more places)
		priority -= parseInt(item.entries ? item.entries : 0) * weights.schoolSize;
		return priority;
	};

	const judges = rawJudges.reduce( (obj, item) => {
		return Object.assign(obj, {
			[item.id] : {
				strikeCount     : 0,
				timeStrikes     : [],
				eventStrikes    : {},
				jpools          : {},
				exclude         : {},
				alreadyCongress : {},
				rounds          : parseInt(item.obligation || 0) + parseInt(item.hired || 0),
				priority        : prioritize(item),
				...item,
			},
		});
	}, {});

	const rawStrikes =  await db.sequelize.query(`
		select
			judge.id judge,
			strike.id strike, strike.type type,
			strike.start,
			strike.end,
			strike.event,
			tourn.end tournEnd
		from (judge, jpool_judge jpj, jpool, category, tourn, strike)

		where jpool.id         = :parentId
			and jpool.id       = jpj.jpool
			and jpj.judge      = judge.id
			and judge.active   = 1
			and judge.category = category.id
			and category.tourn = tourn.id
			and judge.id       = strike.judge
			and strike.type IN ('event', 'time', 'departure')
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	rawStrikes.forEach( (strike) => {

		if (!judges[strike.judge]) {
			return;
		}

		judges[strike.judge].strikeCount++;

		if (strike.type === 'time') {

			const start = new Date(strike.start).valueOf();
			const end   = new Date(strike.end).valueOf();

			Object.keys(jpools).forEach( (jpoolId) => {
				const jpool = jpools[jpoolId];
				if (jpool.start <= end && jpool.end >= start) {
					judges[strike.judge].exclude[jpool.id] = true;
					judges[strike.judge].priority += 10;
				}
			});
		}

		if (strike.type === 'departure') {
			const start = new Date(strike.start).valueOf();

			Object.keys(jpools).forEach( (jpoolId) => {
				const jpool = jpools[jpoolId];
				if (jpool.end >= start) {
					judges[strike.judge].exclude[jpool.id] = true;
					judges[strike.judge].priority += 10;
				}
			});
		}

		if (strike.type === 'event') {
			judges[strike.judge].eventStrikes[strike.event] = true;
			judges[strike.judge].priority += 10;
		}
	});

	const rawJPoolJudges = await db.sequelize.query(`
		select
			judge.id judge,
			jpool.id jpool, jpool.name, rounds.value rounds,
			min(CONVERT_TZ(timeslot.start, '+00:00', tourn.tz)) minStart,
			max(CONVERT_TZ(timeslot.end, '+00:00', tourn.tz)) maxEnd

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

			and not exists (
				select pool_ignore.id
				from jpool_setting pool_ignore
					where pool_ignore.tag = 'pool_ignore'
					and pool_ignore.jpool = jpj.jpool
			)
		group by jpj.id
	`, {
		replacements : { parentId },
		type         : db.sequelize.QueryTypes.SELECT,
	});

	rawJPoolJudges.forEach( (jpj) => {

		if (!judges[jpj.judge]) {
			return;
		}

		if (!judges[jpj.judge].jpools[jpj.jpool]) {

			if (jpj.rounds) {
				judges[jpj.judge].rounds -= parseInt(jpj.rounds);
				judges[jpj.judge].priority += (jpj.rounds * weights.remaining);
			}

			jpj.start = new Date(jpj.minStart).valueOf();
			jpj.end = new Date(jpj.maxEnd).valueOf();

			Object.keys(jpools).forEach( (jpoolId) => {
				const jpool = jpools[jpoolId];
				if (jpool.start <= jpj.end && jpool.end >= jpj.start) {
					judges[jpj.judge].exclude[jpool.id] = true;
				}
			});
		}

	});

	return judges;
};
