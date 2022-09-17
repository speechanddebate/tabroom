import db from '../../../helpers/db';

const selectPanelEmail = async (panel) => {
	const students = await db.sequelize.query(`
		select
			ps.value as 'room',
			person.email as 'email',
			tourn.name as 'tournament',
			COALESCE(NULLIF(round.label, ''), NULLIF(round.name, ''), NULLIF(round.type, ''), 'X') as 'round'
		from panel, panel_setting ps, ballot, round, event, tourn, entry_student es, student, person, entry
		where LOWER(ps.value) = ?
			and ps.tag = 'share'
			and panel.id = ballot.panel
			and ps.panel = panel.id
			and ballot.entry = es.entry
			and round.id = panel.round
			and event.id = round.event
			and tourn.id = event.tourn
			and es.entry = entry.id
			and es.student = student.id
			and student.person = person.id
			and person.no_email != 1
			and round.start_time > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 DAY)
		group by person.id
	`, { replacements: [panel.toLowerCase().replaceAll(/[^a-z0-9]/g, '')] });

	const judges = await db.sequelize.query(`
		select
			ps.value as 'room',
			person.email as 'email',
			tourn.name as 'tournament',
			COALESCE(NULLIF(round.label, ''), NULLIF(round.name, ''), NULLIF(round.type, ''), 'X') as 'round'
		from panel, panel_setting ps, ballot, round, event, tourn, judge, person
		where LOWER(ps.value) = ?
			and ps.tag = 'share'
			and ps.panel = panel.id
			and panel.id = ballot.panel
			and round.id = panel.round
			and event.id = round.event
			and tourn.id = event.tourn
			and ballot.judge = judge.id
			and judge.person = person.id
			and person.no_email != 1
			and round.start_time > DATE_SUB(CURRENT_TIMESTAMP, INTERVAL 1 DAY)
		group by person.id
	`, { replacements: [panel.toLowerCase().replaceAll(/[^a-z0-9]/g, '')] });

	return [...students[0], ...judges[0]];
};

export default selectPanelEmail;
