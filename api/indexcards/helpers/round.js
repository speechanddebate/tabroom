// Common helper functions that attach to rounds & schematics
import db from './db';
import objectify from './objectify';

export const sidelocks = async (roundId) => {

	const sidelockQuery = `
		select
			section.id,
			count(other.id) count,
				neg_bo.side aff_ok,
				aff_bo.side neg_ok

		from section, ballot aff_b, ballot neg_b, entry aff_e, entry neg_e,
			section other, ballot aff_bo, ballot neg_bo

		where section.round = :roundId
			and section.id = aff_b.section
			and aff_b.side = 1
			and aff_b.entry = aff_e.id

			and section.id = neg_b.section
			and neg_b.side = 2
			and neg_b.entry = neg_e.id

			and section.round != other.round

			and other.id = aff_bo.section
			and aff_bo.entry = aff_e.id

			and other.id = neg_bo.section
			and neg_bo.entry = neg_e.id
	`;

	return objectify(await db.sequelize.query(sidelockQuery, {
		replacements : { roundId },
		type         : db.sequelize.QueryTypes.SELECT,
	}));

};

export default sidelocks;
