export const searchTourns = {

	GET: async (req, res) => {

		const db = req.db;

		const replacements = {
			likeString   : `%${req.params.searchString}%`,
			searchString : req.params.searchString,
		};

		let timescale = ' and tourn.start > NOW() ';

		if (req.params.time === 'past') {
			timescale = ' and tourn.start < NOW() ';
		} else if (req.params.time === 'all') {
			timescale = '';
		}

		const tourns = await db.sequelize.query(`
			select
				tourn.id, tourn.webname as abbr, tourn.webname, tourn.name, tourn.city, tourn.state, tourn.tz, 'tourn' as tag,
				CONVERT_TZ(tourn.start, '+00:00', tourn.tz) start,
				CONVERT_TZ(tourn.end, '+00:00', tourn.tz) end,
				tourn.start utc_start,
				GROUP_CONCAT( distinct(circuit.abbr) SEPARATOR ', ') as circuits
			from tourn
				left join tourn_circuit tc on tc.tourn = tourn.id
				left join circuit on tc.circuit = circuit.id
			where tourn.hidden = 0
				and (tourn.name LIKE :likeString OR tourn.name = :searchString OR tourn.webname = :searchString )
				${timescale}
			group by tourn.id
			order by tourn.start
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const circuits = await db.sequelize.query(`
			select
				circuit.id, circuit.name, circuit.abbr, count(circuit.id) as circuits, 'circuit' as tag
			from circuit
				left join tourn on exists (select tc.id from tourn_circuit tc where tc.circuit = circuit.id and tc.tourn = tourn.id)
			where circuit.active = 0
				and (circuit.name LIKE :likeString OR circuit.abbr = :searchString)
			order by circuit.abbr
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const exactMatches = [];
		const partialMatches = [];

		[...tourns, ...circuits].forEach( (result) => {

			if (result
				&& result.id
				&& (
					result.name === req.params.searchString
					|| result.abbr === req.params.searchString
				)
			) {
				exactMatches.push(result);
			} else if (result && result.id) {
				partialMatches.push(result);
			}
		});

		res.status(200).json({ exactMatches, partialMatches, searchString : req.params.searchString });
	},
};

export const searchCircuitTourns = {

	GET: async (req, res) => {

		const db = req.db;
		const replacements = {
			likeString: `%${req.params.searchString}%`,
			circuitId: req.params.circuitId,
		};

		let timescale = ' and tourn.start > NOW() ';

		if (req.params.time && req.params.time === 'past') {
			timescale = ' and tourn.start < NOW() ';
		} else if (req.params.time && req.params.time === 'all') {
			timescale = '';
		}

		const tourns = await db.sequelize.query(`
			select
				tourn.id, tourn.webname, tourn.name, tourn.start, tourn.end, tourn.city, tourn.state, tourn.tz,
				CONVERT_TZ(tourn.start, '+00:00', tourn.tz)
			from tourn
				where tourn.hidden = 0
				and tourn.name LIKE :likeString
				and exists (
					select tourn_circuit.id
					from tourn_circuit
					where tourn_circuit.tourn = tourn.id
					and tourn_circuit.circuit = :circuitId
				)
				${timescale}
			group by tourn.id
			order by tourn.start DESC
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const exactMatches = [];
		const partialMatches = [];

		[tourns].forEach( (result) => {
			if (
				result.name === req.params.searchString
			) {
				exactMatches.push(result);
			} else {
				partialMatches.push(result);
			}
		});

		res.status(200).json({ exactMatches, partialMatches });
	},
};

export default searchTourns;

searchTourns.GET.apiDoc = {
	summary     : 'Function to search for non-hidden tournaments by name in the past, future, or both',
	operationId : 'searchTourns',
	tags        : ['public'],
	parameters  : [
		{
			in          : 'path',
			name        : 'time',
			description : 'Time limiter',
			required    : true,
			schema      : {
				type : 'string',
				enum : ['all', 'past', 'future'],
			},
		},{
			in          : 'path',
			name        : 'searchString',
			description : 'Search String',
			required    : true,
			schema      : {
				type    : 'string',
			},
		},
	],
	responses: {
		200: {
			description: 'Search',
			content: { '*/*': { schema: { $ref: '#/components/schemas/Search' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
};

searchCircuitTourns.GET.apiDoc = {
	summary     : 'Function to search for non-hidden tournaments in a circuit by name',
	operationId : 'searchCircuitTourns',
	tags        : ['public'],
	parameters: [
		{
			in          : 'path',
			name        : 'circuitId',
			description : 'ID of the circuit',
			required    : true,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},{
			in          : 'path',
			name        : 'searchString',
			description : 'Search String',
			required    : true,
			schema      : {
				type    : 'string',
			},
		},
	],
	responses: {
		200: {
			description: 'Search',
			content: { '*/*': { schema: { $ref: '#/components/schemas/Search' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
};
