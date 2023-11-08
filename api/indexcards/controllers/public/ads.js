export const getAds = {

	GET: async (req, res) => {

		const db = req.db;

		const currentAds = await db.sequelize.query(`
			select id, filename, url
				from ad
			where ad.start < NOW()
				and ad.end > NOW()
				and ad.approved = 1
			order by ad.sort_order, RAND()
		`, {
			type: db.sequelize.QueryTypes.SELECT,
		});

		res.status(200).json(currentAds);
	},
};

export default getAds;

getAds.GET.apiDoc = {
	summary     : 'Return list of ads to display on front page',
	operationId : 'getAds',
	tags        : ['public'],
	responses: {
		200: {
			description: 'Ads',
			content: {
				'*/*': {
					schema: {
						type: 'array',
						items: { $ref: '#/components/schemas/Ad' },
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
};
