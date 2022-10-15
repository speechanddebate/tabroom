export const futureTourns = {
	GET: async (req, res) => {

		const db = req.db;
		let limit = '';

		if (typeof req.params.circuit === 'number') {
			limit = ` and exists (
				select tourn_circuit.id from tourn_circuit
				where tourn_circuit.tourn = tourn.id
				and tourn_circuit.approved = 1
				and tourn_circuit.circuit = ${ req.params.circuit } ) `;
		}

		const [futureTourns] = await db.sequelize.query(`
			select tourn.id, tourn.webname, tourn.name, tourn.tz, tourn.hidden,
				tourn.state, tourn.country, tourn.city,
				CONVERT_TZ(tourn.start, '+00:00', tourn.tz) start,
				CONVERT_TZ(tourn.end, '+00:00', tourn.tz) end,
				CONVERT_TZ(tourn.reg_end, '+00:00', tourn.tz) reg_end,
				CONVERT_TZ(tourn.reg_start, '+00:00', tourn.tz) reg_start,
				msnats.value as msnats,
				nats.value as nats,
				closed.value as closed,
				count(distinct school.id) as schoolcount,
				count(distinct online.id) as online,
				count(distinct inperson.id) as inperson,
				YEAR(tourn.start) as year,
				WEEK(tourn.start) as week,
				GROUP_CONCAT(distinct(signup.abbr) SEPARATOR ', ') as signup

			from tourn

			left join tourn_setting closed
				on closed.tourn = tourn.id
				and closed.tag = 'closed_entry'

			left join tourn_setting msnats
				on msnats.tourn = tourn.id
				and msnats.tag = 'nsda_ms_nats'

			left join tourn_setting nats
				on nats.tourn = tourn.id
				and nats.tag = 'nsda_nats'

			left join event online
				on online.tourn = tourn.id
				and exists (
					select eso.id
					from event_setting eso
					where eso.event = online.id
					and eso.tag = 'online_mode'
				)

			left join event inperson
				on inperson.tourn = tourn.id
				and not exists (
					select inpes.id
					from event_setting inpes
					where inpes.event = inperson.id
					and inpes.tag = 'online_mode'
				)

			left join school on tourn.id = school.tourn

			left join category signup
				on signup.tourn = tourn.id
				and signup.abbr IS NOT NULL
				and signup.abbr != ''
				and exists ( select cs.id
					from category_setting cs
					where cs.category = signup.id
					and cs.tag = 'public_signups'
				)
				and exists (
					select csd.id
					from category_setting csd
					where csd.category = signup.id
					and csd.tag = 'public_signups_deadline'
					and csd.value_date > NOW()
				)
				and not exists (
					select csd.id
					from category_setting csd
					where csd.category = signup.id
					and csd.tag = 'private_signup_link'
				)

			where tourn.hidden = 0
			and tourn.end > DATE(NOW() - INTERVAL 2 DAY)
			${ limit }
			and not exists (
				select weekend.id
				from weekend
				where weekend.tourn = tourn.id
			)
			group by tourn.id
			order by tourn.end, schoolcount DESC
		`);

		const [futureDistricts] = await db.sequelize.query(`
			select
				tourn.id, tourn.webname, tourn.name, tourn.tz,
				weekend.name weekendName, weekend.city, weekend.state,
				site.name site,
				CONVERT_TZ(weekend.start, '+00:00', tourn.tz) start,
				CONVERT_TZ(weekend.end, '+00:00', tourn.tz) end,
				CONVERT_TZ(weekend.reg_end, '+00:00', tourn.tz) reg_end,
				CONVERT_TZ(weekend.reg_start, '+00:00', tourn.tz) reg_start,
				count(distinct school.id) as schoolcount,
				YEAR(weekend.start) as year,
				WEEK(weekend.start) as week

			from (tourn, weekend)

			left join site on weekend.site = site.id
			left join school on tourn.id = school.tourn

			where tourn.hidden = 0
			and weekend.end > DATE(NOW() - INTERVAL 2 DAY)
			and weekend.tourn = tourn.id

			group by weekend.id
			order by weekend.start
		`);

		futureTourns.push(...futureDistricts);
		return res.status(200).json(futureTourns);
	},
};

futureTourns.GET.apiDoc = {
	summary: 'Returns the public listing of upcoming tournaments',
	operationId: 'futureTourns',
	parameters: [
		{
			in          : 'path',
			name        : 'circuit',
			description : 'ID of a circuit to limit the search to',
			required    : false,
			schema      : { type: 'integer', minimum : 1 },
		},
	],
	responses: {
		200: {
			description: 'List of public upcoming tournaments',
			content: { '*/*': { schema: { $ref: '#/components/schemas/futureTourns' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['futureTourns', 'invite', 'public'],
};
