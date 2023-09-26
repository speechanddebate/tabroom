// This begins life as the Texas Forensic Association IQT formula but I expect
// others may find it useful someday.

export const tournQualifierResult = {
	GET: async (req, res) => {
		const db = req.db;

		const events = await db.event.findAll(
			{ where: { tourn: req.params.tourn_id } }
		);

		events.forEach( async (event) => {
			await saveEventResult(req.db, event.id);
		});

		res.status(200).json({
			error: false,
			message: `Tournament qualifying data posted.`,
		});
	},
};

export const eventQualifierResult = {
	GET: async (req, res) => {
		const message = await saveEventResult(req.db, req.params.event_id);

		res.status(200).json({
			error: false,
			message,
		});

	},
};

const saveEventResult = async (db, eventId) => {
	// Get event and qualifier event tags

	const eventQuery = `
	select
		event.id, tc.circuit, circuit.abbr circuitAbbr,
			ruleset.value rulesetId,
			qual_event.value eventCode,
			count(distinct entry.id) entryCount,
			count(distinct entry.school) schoolCount,
			cr.value_text circuitRules

	from (event, tourn, tourn_circuit tc, circuit_setting cr)

		left join circuit on circuit.id = tc.circuit

		left join event_setting ruleset
			on ruleset.event = event.id
			and ruleset.tag = CONCAT('qualifier_', tc.circuit)

		left join event_setting qual_event
			on qual_event.event = event.id
			and qual_event.tag = CONCAT('qualifier_event', tc.circuit)

		left join entry
			on entry.event = event.id
			and exists (
				select ballot.id
					from ballot, panel
				where ballot.entry = entry.id
					and ballot.bye != 1
					and ballot.forfeit != 1
					and ballot.panel = panel.id
					and panel.bye != 1
			)
	where event.id = :eventId
		and event.tourn = tourn.id
		and tourn.id = tc.tourn
		and tc.circuit = cr.circuit
		and cr.tag = 'qualifiers'
		and cr.value = 'json'
	group by event.id, tc.circuit
	`;

	const eventsWithQualifiers = await db.sequelize.query(eventQuery, {
		replacements: { eventId },
		type: db.sequelize.QueryTypes.SELECT,
	});

	let message = '';

	eventsWithQualifiers.forEach( async (event) => {

		const allRules = JSON.parse(event.circuitRules);
		const eventRules = allRules[event.rulesetId];
		if (!eventRules) {
			return;
		}

		// Choose the correct rule subset based on the size of the entry
		// field or school count

		const margins = {
			school : 0,
			entry  : 0,
		};

		let qualRuleSet = {};

		Object.keys(eventRules.rulesets).forEach( async (rulesetTag) => {

			const ruleset = eventRules.rulesets[rulesetTag];

			// I'm not over the entry threshold
			if (event.entryCount < ruleset.entries) {
				return;
			}

			// I'm not over the school threshold
			if (event.schoolCount < ruleset.schools) {
				return;
			}

			// I'm over a different, higher threshold already
			if (qualRuleSet) {
				if ( (event.schoolCount - ruleset.schoolCount) > margins.school) {
					return;
				}
				if ( (event.entryCount - ruleset.entryCount) > margins.entry) {
					return;
				}
			}

			qualRuleSet = ruleset;
			margins.school = event.schoolCount - ruleset.schoolCount;
			margins.entry = event.entryCount - ruleset.entryCount;
		});

		// Create results set, wiping out any existing ones, for this event & circuit.
		await db.resultSet.destroy({
			where: {
				event   : eventId,
				circuit : event.circuit,
			},
		});

		const newResultSet = await db.resultSet.create({
			circuit   : event.circuit,
			event     : eventId,
			tag       : 'entry',
			label     : `${event.circuitAbbr} Qualification`,
			code      : event.eventCode,
			generated : new Date(),
		});

		// Get final results set for the rankings

		const finalResultQuery = `
				select
				result.entry, result.rank
			from result, result_set
			where result_set.event = :eventId
				and result_set.label = 'Final Places'
				and result_set.id = result.result_set
			order by result.rank
		`;

		const finalResults = await db.sequelize.query(finalResultQuery, {
			replacements: { eventId },
			type: db.sequelize.QueryTypes.SELECT,
		});

		const entryByRank = {};
		for (const result of finalResults) {
			if (!entryByRank[result.rank]) {
				entryByRank[result.rank] = [];
			}
			entryByRank[result.rank].push(result.entry);
		}

		// Get last round participated data

		const lastRoundQuery = `
			select entry.id, max(round.name) round
				from entry, ballot, panel, round
			where entry.event = :eventId
				and entry.id = ballot.entry
				and ballot.panel = panel.id
				and panel.round = round.id
				and round.event = :eventId
			group by entry.id
		`;

		const lastRound = await db.sequelize.query(lastRoundQuery, {
			replacements: { eventId },
			type: db.sequelize.QueryTypes.SELECT,
		});

		const entriesByLastRound = {};

		for (const entryRound of lastRound) {

			if (!entriesByLastRound[entryRound.round]) {
				entriesByLastRound[entryRound.round] = [];
			}
			entriesByLastRound[entryRound.round].push(entryRound.entry);
		}

		const allElims = await db.sequelize.query(`
			select round.name 
				from round 
			where round.event = :eventId 
				and round.type IN ('elim', 'final')
			ORDER BY round.name
		`, {
			replacements: { eventId },
			type: db.sequelize.QueryTypes.SELECT,
		});

		const entryPoints = {};

		for (const key of Object.keys(qualRuleSet.rules)) {
			const rule = qualRuleSet.rules[key];

			// Award points for the final result placements and save
			if (rule.placement > 0) {
				for (const entry of entryByRank[rule.placement]) {
					entryPoints[entry] = rule.points;
				}
			}

			// Award points for the last elimination placed unless a rank
			// placement supercedes it

			if (rule.reverse_elim > 0) {
				const targetRound = allElims[(rule.reverse_elim * -1)];

				if (targetRound) {
					for (const entry of entriesByLastRound[targetRound]) {
						if (!entryPoints[entry]) {
							entryPoints[entry] = rule.points;
						}
					}
				}
			}
		}

		// Save the award points
		Object.keys(entryPoints).forEach( async (entry) => {
			console.log(`Saving points ${entryPoints[entry]} for entry ${entry} into result set ${newResultSet.id}`);

			const result = await db.result.create({
				result_set : newResultSet.id,
				rank      : entryPoints[entry],
				entry,
			});

			console.log(result);
		});

		message = `${event.circuitAbbr} qualifying results in ${event.eventCode} have been generated`;
	});

	return message;
};

export default eventQualifierResult;
