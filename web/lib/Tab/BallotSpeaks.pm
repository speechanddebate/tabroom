package Tab::Ballot;
use base 'Tab::DBI';
Tab::Ballot->table('ballot');
Tab::Ballot->columns(Primary => qw/id/);
Tab::Ballot->columns(Essential => qw/judge panel entry rank real_rank countmenot
									 points real_points speakerorder bye
									 timestamp speechnumber topic collected
									 collected_by audit rankinround win side flight/);
Tab::Ballot->columns(Others => qw/tv noshow seed chair/);
Tab::Ballot->columns(TEMP => qw/code/);
Tab::Ballot->has_a(judge => 'Tab::Judge');
Tab::Ballot->has_a(panel => 'Tab::Panel');
Tab::Ballot->has_a(entry => 'Tab::Entry');
Tab::Ballot->has_a(collected_by => 'Tab::Account');

__PACKAGE__->_register_dates( qw/timestamp collected/);

Tab::Ballot->set_sql(remove_chairs => "update ballot set chair = 0 where panel = ?");
Tab::Ballot->set_sql(add_chair => "update ballot set chair = 1 where panel = ? and judge = ?");


Tab::Ballot->set_sql(count_unchecked => "select count(distinct ballot.judge)
											from ballot,panel,round
											where ballot.collected_by = 0 
											and ballot.panel = panel.id
											and panel.round = round.id
											and round.timeslot = ?");

Tab::Ballot->set_sql(speech_ballots_by_tourn => "select distinct ballot.*
							from ballot,entry,event
							where entry.event = event.id
							and event.tourn = ?
							and event.type = \"speech\"
							and ballot.entry = entry.id");

Tab::Ballot->set_sql(by_timeslot => "
						select distinct ballot.*
						from ballot,panel,round
						where ballot.panel = panel.id
						and panel.round = round.id
						and round.timeslot = ?");

Tab::Ballot->set_sql(set_question_by_timeslot => "
						update ballot,panel,round
						set ballot.topic = ?
					    where ballot.entry = ?
						and ballot.panel = panel.id
						and panel.round = round.id
						and round.timeslot = ?");

Tab::Ballot->set_sql(question_by_timeslot => "
						select ballot.topic 
						from ballot,panel,round
					    where ballot.entry = ?
						and ballot.panel = panel.id
						and panel.round = round.id
						and round.timeslot = ? ");

Tab::Ballot->set_sql(judge_busy_during => "
				select ballot.id from ballot,panel,round,timeslot as btime,timeslot as ctime
					where ballot.judge = ? 
					and ballot.panel = panel.id
					and panel.round = round.id
					and round.timeslot = btime.id
					and ctime.id = ? 
					and ctime.start < btime.end
					and ctime.end > btime.start
				");

Tab::Ballot->set_sql(undone_by_timeslot => "
		select ballot.id from ballot,entry,panel,round
			where ballot.panel = panel.id
	        and ballot.entry = entry.id
			and panel.round = round.id
			and round.timeslot = ? 
	        and entry.dropped != 1
			and ballot.audit = 0 ");

Tab::Ballot->set_sql(undone_by_panel => "
		select ballot.* from ballot,entry
			where ballot.panel = ?
	        and ballot.entry = entry.id
	        and entry.dropped = 0
			and ballot.audit = 0 ");

Tab::Ballot->set_sql(undone_by_panel_and_judge => "
		select ballot.* from ballot,entry
			where ballot.panel = ?
			and ballot.judge = ?
	        and ballot.entry = entry.id
	        and entry.dropped = 0
			and ballot.audit = 0
			"
			);

Tab::Ballot->set_sql(by_round => "
		select ballot.* from ballot,panel
			where panel.round = ? 
			and ballot.panel = panel.id
			order by ballot.judge");

Tab::Ballot->set_sql(ordered_by_round => "
		select ballot.* from ballot,panel,round
			where ballot.entry = ?
			and ballot.panel = panel.id
			and panel.round = round.id 
			order by round.name,ballot.judge");

Tab::Ballot->set_sql(elim_by_round => "
		select ballot.* from ballot,panel,round
			where ballot.entry = ?
			and ballot.panel = panel.id
			and panel.round = round.id 
			and panel.type != \"prelim\"
			order by round.name,ballot.judge");

Tab::Ballot->set_sql(prelim_by_round => "
		select ballot.* from ballot,panel,round
			where ballot.entry = ?
			and ballot.panel = panel.id
			and panel.round = round.id 
			and panel.type = \"prelim\"
			order by round.name,ballot.judge");

Tab::Ballot->set_sql(judge_timeslot => "
		select distinct ballot.id from ballot,panel,round,timeslot,entry
			where ballot.judge = ?
			and ballot.panel = panel.id
			and panel.round = round.id
			and round.timeslot = ? 
			and entry.id = ballot.entry
			and entry.dropped = 0
		");

Tab::Ballot->set_sql(round_ballots => "
			select distinct ballot.id from ballot,round,panel
				where ballot.entry = ? 
				and ballot.panel = panel.id
				and panel.round = ? ");

Tab::Ballot->set_sql(by_tourn => "
			select distinct ballot.id from ballot,event,panel
				where ballot.panel = panel.id
				and panel.event = event.id
				and event.tourn = ? ");

Tab::Ballot->set_sql(ballots_from_prelims => "
			select distinct ballot.id from ballot,panel
			where ballot.entry = ? 
			and ballot.panel = panel.id
			and panel.type = \"prelim\" ");

Tab::Ballot->set_sql(ballots_from_elims => "
			select distinct ballot.id from ballot,panel
			where ballot.entry = ? 
			and ballot.panel = panel.id
			and panel.type = \"elim\" ");

Tab::Ballot->set_sql(ballots_from_finals => "
			select distinct ballot.id from ballot,panel
			where ballot.entry = ? 
			and ballot.panel = panel.id
			and panel.type = \"final\" ");

Tab::Ballot->set_sql(ballots_by_event => "
			select distinct ballot.*,entry.code as code
			from ballot,entry,panel,round
			where ballot.entry = entry.id
			and entry.event = ?
			and entry.dropped = 0
			and ballot.panel = panel.id
			and panel.round = round.id
			order by round.name,ballot.judge");

Tab::Ballot->set_sql(prelim_ballots_by_event => "
			select distinct ballot.*,entry.code as code
			from ballot,entry,panel,round
			where ballot.entry = entry.id
			and entry.event = ?
			and entry.dropped = 0
			and ballot.panel = panel.id
			and panel.round = round.id
			and round.type = \"prelim\"
			order by round.name,ballot.judge");

Tab::Ballot->set_sql(empty_by_round =>"
			select distinct ballot.id from ballot,panel
			where ballot.entry is null
			and ballot.panel = panel.id 
			and panel.round = ?");

Tab::Ballot->set_sql(empty_by_panel =>"
			select distinct ballot.id from ballot
			where ballot.entry is null
			and ballot.panel = ?");
