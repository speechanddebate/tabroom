import { checkJudgePerson } from '../../../helpers/tourn-auth';

export const saveRubric = {

	POST: async (req, res) => {

		const db = req.db;
		const autoSave = req.body;
		const judgeId = parseInt(req.params.judge_id);

		// putting the judgeId into parameters and not the body because
		// eventually I'll want to put these access checks up the chain

		const judgeOK = await checkJudgePerson(req, judgeId);

		if (!judgeOK) {
			res.status(200).json({
				error: true,
				message: 'You do not have permissions to change that ballot',
			});
		} else {

			const ballot = await db.summon(db.ballot, autoSave.ballot);

			if (ballot.judge !== judgeId) {
				res.status(200).json({
					error   : true,
					message : `You are not the listed judge for that ballot.  ${ballot.judge} vs ${judgeId}`,
				});
			} else {

				const score = await db.score.findOne({ where: { ballot: ballot.id, tag: 'rubric' } });
				delete autoSave.ballot;

				if (score && score.id) {

					try {
						score.content = JSON.stringify(autoSave);
						await score.save();
					} catch (err) {
						res.status(200).json({
							error: true,
							message: `Error encounted in saving scores: ${err}`,
						});
					}

				} else {

					try {
						await db.score.create({
							ballot  : ballot.id,
							tag     : 'rubric',
							value   : 0,
							content : JSON.stringify(autoSave),
						});
					} catch (err) {
						res.status(200).json({
							error: true,
							message: `Error encounted in creating saved scores: ${err}`,
						});
					}
				}

				res.status(200).json({
					error: false,
					message: `Scores auto-saved!`,
				});
			}
		}
	},
};

export default saveRubric;
