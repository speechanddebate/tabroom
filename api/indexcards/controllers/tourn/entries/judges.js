export const listJudges = {
    GET: async (req, res) => {
		const db      = req.db;
		const tournId = req.params.tourn_id;
		const op      = db.Sequelize.Op

		const judgeQuery = "select judge.* from (category, judge) where category.tourn = ? and category.id = judge.category";

		// A raw query to go through the category filter
		db.sequelize.query(
			judgeQuery,
			{ replacements: [tournId] }
		).then(judges => {
			return res.status(200).json(judges);
		});
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
    tags: ['tournament/entries'],
};

