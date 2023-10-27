// import { showDateTime } from '../../../helpers/common';

export const eraseJPool = {

	POST: async (req, res) => {
		await req.db.sequelize.query(`
			delete jpj.* from jpool_judge jpj where jpj.jpool = :jpoolId
		`, {
			replacements: { jpoolId: req.params.jpoolId },
			type: req.db.sequelize.QueryTypes.DELETE,
		});

		res.status(200).json({
			error   : false,
			refresh : true,
			message : 'All judges dumped',
		});
	},
};

export const populateStandby = {

	POST: async (req, res) => {

		let rawJudges = [];

		if (req.body.parentId && req.body.parentId !== 'NaN') {

			const standbyJudgeQuery = `
				select
					judge.id, judge.school, region.id region,
						count(distinct panel.id) ballots,
						count(distinct standby.jpool) standbys,
						count(distinct entry.id) entries
				from (judge, jpool_judge jpj)

					left join jpool_judge standby
						on standby.judge = judge.id
						AND EXISTS (
							select jps.id
							from jpool_setting jps
							where jps.jpool = standby.jpool
							and jps.tag = 'standby')

					left join ballot on ballot.judge = judge.id
					left join panel on panel.id = ballot.panel

					left join school on judge.school = school.id
					left join region on school.region = region.id
					left join entry on entry.school = school.id and entry.active = 1

				where jpj.jpool = :parentId
					and jpj.judge = judge.id
				group by judge.id
				order by standbys desc, ballots desc
			`;

			rawJudges = await req.db.sequelize.query(standbyJudgeQuery, {
				replacements: { parentId: req.body.parentId },
				type: req.db.sequelize.QueryTypes.SELECT,
			});

		} else if (req.body.categoryId) {

			const standbyJudgeQuery = `
				select
					judge.id, judge.school, region.id region,
						count(distinct panel.id) ballots,
						count(distinct standby.jpool) standbys,
						count(distinct entry.id) entries
				from (judge)

					left join jpool_judge standby
						on standby.judge = judge.id
						AND EXISTS (
							select jps.id
							from jpool_setting jps
							where jps.jpool = standby.jpool
							and jps.tag = 'standby')

					left join ballot on ballot.judge = judge.id
					left join panel on panel.id = ballot.panel

					left join school on judge.school = school.id
					left join region on school.region = region.id
					left join entry on entry.school = school.id and entry.active = 1

				where judge.category = :categoryId
				group by judge.id
				order by standbys desc, ballots desc
			`;

			rawJudges = await req.db.sequelize.query(standbyJudgeQuery, {
				replacements: { categoryId: req.body.categoryId },
				type: req.db.sequelize.QueryTypes.SELECT,
			});
		}

		const chosen = {};

		for await (const judge of rawJudges) {
			judge.score = (parseInt(judge.standbys) * 100);
			judge.score += (parseInt(judge.ballots) * 10);
			judge.score += parseInt(judge.entries);
		}

		let counter = req.body.targetCount;

		while (counter > 0) {
			counter--;
			rawJudges.sort( (a, b) => parseInt(a.score) - parseInt(b.score));
			const picked = rawJudges.shift();

			if (picked) {
				chosen[picked.id] = picked;

				for (const judge of rawJudges) {
					if (judge.school > 0 && judge.school === picked.school) {
						judge.score += 5000;
					}
					if (judge.region > 0 && judge.region === picked.region) {
						judge.score += 1000;
					}
				}
			}
		}

		const addJudge = `insert into jpool_judge (judge, jpool) values (:judgeId, :jpoolId)`;

		Object.keys(chosen).forEach( async (judgeId) => {

			await req.db.sequelize.query(addJudge, {
				replacements: { judgeId, jpoolId: req.body.jpoolId },
				type: req.db.sequelize.QueryTypes.INSERT,
			});
		});

		res.status(200).json({
			error   : false,
			refresh : true,
			message : 'Judges added to standby pool',
		});

	},
};

export default populateStandby;
