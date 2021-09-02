export const listEntries = {
	GET: async (req, res) => {
		const db      = req.db;
		const tournId = req.params.tourn_id;

		const entryQuery = `
			select entry.*,
			student.id as student_id,
			student.first as student_first,
			student.middle as student_middle,
			student.last as student_last,
			student.nsda as student_nsda
				from (event, entry)
				left join entry_student es on es.entry = entry.id
				left join student on es.student = student.id
			where event.tourn = :tourn
				and event.id = entry.event
				and entry.unconfirmed = 0
		`;

		// A raw query to go through the event filter
		const results = await db.sequelize.query(
			entryQuery,
			{ replacements: { tourn: tournId } }
		);

		const entries = {};

		results[0].forEach( entry => {

			const student = {
				id     : entry.student_id,
				first  : entry.student_first,
				middle : entry.student_middle,
				last   : entry.student_last,
				nsda   : entry.student_nsda,
			};

			if (entries[entry.id]) {

				entries[entry.id].students.push(student);

			} else {

				entries[entry.id] = entry;

				// this feels sufficiently annoying I think there must be a
				// better way, though it does beat the perl requirement to
				// populate all the fields I did want instead of eliminating
				// those I did not.

				delete entries[entry.id].student_id;
				delete entries[entry.id].student_first;
				delete entries[entry.id].student_middle;
				delete entries[entry.id].student_last;
				delete entries[entry.id].student_nsda;
				entries[entry.id].students = [student];
			}
		});

		return res.status(200).json(entries);
	},
};

export default listEntries;

listEntries.GET.apiDoc = {
	summary: 'Listing of entries in the tournament',
	operationId: 'listEntries',
	parameters: [
		{
			in          : 'path',
			name        : 'tourn_id',
			description : 'Tournament ID',
			required    : true,
			schema      : {
				type    : 'integer',
				minimum : 1,
			},
		},
	],
	responses: {
		200: {
			description: 'Entry Data',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Entry' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['tournament/register'],
};
