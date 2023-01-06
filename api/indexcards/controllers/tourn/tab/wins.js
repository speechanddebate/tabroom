// stub for now because apparently I bit off more than I wanted to today

export const entryWins = {
	GET: async (req, res) => {
		const db = req.db;
		const entryId = req.params.entryId;
		console.log(db);
		console.log(entryId);
	},
};

export const eventWins = {
	GET: async (req, res) => {
		const db = req.db;
		const eventId = req.params.eventId;
		console.log(db);
		console.log(eventId);
	},
};
