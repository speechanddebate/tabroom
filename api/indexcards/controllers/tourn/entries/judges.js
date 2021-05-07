export const listJudges = {
    GET: async (req, res) => {
		const db      = req.db;
		const tournId = req.params.tourn_id;
		const op      = db.Sequelize.Op

		const judgeQuery = `
			select judge.*,
			person.id as person_id,
			person.email as person_email,
			person.no_email as person_no_email,
			person.phone as person_phone,
			person.provider as person_provider,
			person.nsda as person_nsda
				from (category, judge)
				left join person account on judge.person = account.id
			where category.tourn = :tourn
				and category.id = judge.category
		`;

		// A raw query to go through the category filter
		var results = await db.sequelize.query(
			judgeQuery,
			{ replacements: {tourn: tournId }}
		);

		let judges = {};

		for (let judge of results[0]) {

			let person = {
				id       : judge.person_id,
				email    : judge.person_email,
				no_email : judge.person_no_email,
				phone    : judge.person_phone,
				provider : judge.person_provider,
				nsda     : judge.person_nsda
			};

			if (judges[judge.id]) {

				judges[judge.id].account.push(person);

			} else {

				judges[judge.id] = judge;

				// this feels sufficiently annoying I think there must be a
				// better way, though it does beat the perl requirement to
				// populate all the fields I did want instead of eliminating
				// those I did not.

				delete judges[judge.id].person_id;
				delete judges[judge.id].person_email;
				delete judges[judge.id].person_no_email;
				delete judges[judge.id].person_phone;
				delete judges[judge.id].person_provider;
				delete judges[judge.id].person_nsda;

				judges[judge.id].account = [person];
			}
		}

		return res.status(200).json(judges);
    },
};

listJudges.GET.apiDoc = {
    summary: 'Listing of judges in the tournament',
    operationId: 'listJudges',
    parameters: [
        {
            in          : 'path',
            name        : 'tourn_id',
            description : 'Tournament ID',
            required    : true,
            schema      : {
				type    : 'integer',
				minimum : 1
			},
        },
    ],
    responses: {
        200: {
            description: 'Judge Data',
            content: {
                '*/*': {
                    schema: {
                        type: 'array',
                        items: { $ref: '#/components/schemas/Judge' },
                    },
                },
            },
        },
        default: { $ref: '#/components/responses/ErrorResponse' },
    },
    tags: ['tournament/judges'],
};

