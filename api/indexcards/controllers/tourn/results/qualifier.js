// This begins life as the Texas Forensic Association IQT formula but I expect
// others may find it useful someday.

export const qualifierResult = {
	GET: async (req, res) => {

		const db = req.db;
		// Get event and qualifier event tags

		const eventQuery = `
			select
				event.id, tc.circuit, 
					ruleset.value rulesetId, 
					qual_event.value eventCode, 
					count(distinct entry.id) entryCount,
					count(distinct entry.school) schoolCount,
					cr.value_text circuitRules

			from (event, tourn, tourn_circuit tc, circuit_setting cr)
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
			replacements: { eventId:  req.params.event_id },
			type: db.sequelize.QueryTypes.SELECT,
		});

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

			let chosenRule = {};

			Object.keys(eventRules.rulesets).forEach( async (rulesetTag) => {

				const ruleset = eventRules.rulesets[rulesetTag];

				console.log(ruleset);
				console.log(`Testing ${rulesetTag}`);
				console.log(`Event count is ${event.entryCount} and ruleset threshold is ${ruleset.entries}`);

				// I'm not over the entry threshold
				if (event.entryCount < ruleset.entries) {
					return;
				}

				// I'm not over the school threshold
				if (event.schoolCount < ruleset.schools) {
					return;
				}

				// I'm over a different, higher threshold already
				if (chosenRule) {
					if ( (event.schoolCount - ruleset.schoolCount) > margins.school) {
						return;
					}
					if ( (event.entryCount - ruleset.entryCount) > margins.entry) {
						return;
					}
				}

				chosenRule = ruleset;
				margins.school = event.schoolCount - ruleset.schoolCount;
				margins.entry = event.entryCount - ruleset.entryCount;
			});

			console.log(`Chosen rule is ${chosenRule}`);
			res.json(chosenRule);

			// Get final results set
			const finalResultQuery = `
				select
					result_set.id setId, result.id resultId, result.entry, result.rank
				from result, result_set
				where result_set.event = :eventId
					and result_set.label = 'Final Places'
					and result_set.id = result.result_set
			`;

			const finalResults = await db.sequelize.query(finalResultQuery, {
				replacements: { eventId:  req.params.event_id },
				type: db.sequelize.QueryTypes.SELECT,
			});

			// Create results set, wiping out any existing ones, for this event & circuit.
			const wipeRs = `
				delete from result_set where result_set.event = :eventId
					and result_set.circuit = :circuitId
			`;

			await db.sequelize.query(wipeRs, {
				replacements: {
					eventId:  req.params.event_id,
					circuitId: event.circuit
				},
				type: db.sequelize.QueryTypes.DELETE,
			});

			// Award points for the final result placements and save

			// Award points for the reverse elimination round participants and save

		});

	},
};

export default qualifierResult;
