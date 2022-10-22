export const searchAttendees = {

	GET: async (req, res) => {

		if (typeof req.params.searchString !== 'string') {
			res.status(201).json({ message: 'Nothing to search' });
		}

		const db      = req.db;

		const replacements = {
			tournId: req.params.tourn_id,
			searchString: req.params.searchString,
			likeString: `${req.params.searchString}%`,
		};

		const entries = await db.sequelize.query(`
			select 
				student.id, student.first, student.last, 
				entry.id entry, entry.code, entry.name, event.abbr, school.name schoolName, school.id schoolId, 'entry' as tag
			from (entry, event, entry_student es, student)
				left join school on school.id = entry.school
			where event.tourn = :tournId
				and event.id = entry.event
				and entry.unconfirmed = 0
				and entry.id = es.entry
				and es.student = student.id
				and (student.last LIKE :likeString OR entry.code LIKE :likeString)
			group by entry.id
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const judges = await db.sequelize.query(`
			select 
				judge.id, judge.code, judge.first, judge.last, category.abbr, school.name schoolName, school.id schoolId, 'judge' as tag
			from (judge, category)
				left join school on school.id = judge.school
			where category.tourn = :tournId
				and category.id = judge.category
				and (judge.last LIKE :likeString OR judge.code = :searchString)
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const schools = await db.sequelize.query(`
			select 
				school.id, school.code, school.name, count(distinct entry.id) as entries, count(distinct judge.id) as judges, 'school' as tag
			from school
				left join entry on entry.school = school.id and entry.unconfirmed = 0
				left join judge on judge.school = school.id
			where school.tourn = :tournId
				and (school.name LIKE :likeString OR school.code = :searchString)
			group by school.id
		`, {
			replacements,
			type: db.sequelize.QueryTypes.SELECT,
		});

		const exactMatches = [];
		const partialMatches = [];

		[...entries, ...judges, ...schools].forEach( (result) => {
			if (
				(result.code && result.code === req.params.searchString)
				|| (result.last && result.last === req.params.searchString)
				|| result.name === req.params.searchString
			) {
				exactMatches.push(result);
			} else {
				partialMatches.push(result);
			}
		});

		res.status(200).json({ exactMatches, partialMatches });
	},
};

export default searchAttendees;
