export const getInvite = {
	GET: async (req, res) => {
		const db = req.db;

		const invite = {};

		if (parseInt(req.params.webname)) {

			invite.tourn = await db.tourn.findByPk(
				parseInt(req.params.tourn_id), {
					include: [
						{ model: db.tournSetting, as: 'Settings' },
						{ model: db.webpage, as: 'Webpages' },
						{ model: db.event, as: 'Events' },
						{ model: db.file, as: 'Files' },
					],
				});

		} else if (req.params.webname) {

			invite.tourn = await db.tourn.findOne({
				where : { webname: req.params.webname },
				order:  [['start', 'desc']],
				include: [
					{ model: db.tournSetting, as: 'Settings' },
					{ model: db.webpage, as: 'Webpages' },
					{ model: db.event, as: 'Events' },
					{ model: db.file, as: 'Files' },
				],
			});
		}

		return res.status(200).json(invite);
	},
};

export const getRounds = {
	GET: async (req, res) => {
		const db = req.db;

		let schemat = {};

		if (parseInt(req.params.round_id)) {

			schemat = await db.round.findByPk(
				parseInt(req.params.round_id), {
					include: [
						{ model: db.roundSetting, as: 'Settings' },
						{ model: db.panel, as: 'Panels' },
					],
				});

			if (schemat && schemat.published === 0) {
				schemat = { message: 'Round is not published' };
			}

			if (schemat == null) {
				schemat = { message: 'Round is not published' };
			}
		}

		return res.status(200).json(schemat);
	},
};

getInvite.GET.apiDoc = {
	summary: 'Returns the public pages for a tournament',
	operationId: 'getInvite',
	parameters: [
		{
			in: 'path',
			name: 'webname',
			description: 'Public webname of the tournament to return',
			required: false,
			schema: { type: 'string', minimum: 1 },
		},
		{
			in: 'path',
			name: 'tourn_id',
			description: 'Tournament ID of tournament to return',
			required: false,
			schema: { type: 'integer', minimum: 1 },
		},
	],
	responses: {
		200: {
			description: 'Invitationl & General Tournament Info',
			content: { '*/*': { schema: { $ref: '#/components/schemas/Invite' } } },
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
	tags: ['invite', 'public'],
};
