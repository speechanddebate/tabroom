package Tab::Panel;
use base 'Tab::DBI';
Tab::Panel->table('panel');
Tab::Panel->columns(Primary => qw/id/);
Tab::Panel->columns(Essential => qw/event timestamp letter type room round nosweep flight bye/);
Tab::Panel->columns(TEMP =>   qw/tmp_score num_judges/);
Tab::Panel->has_a(event => 'Tab::Event');
Tab::Panel->has_a(room => 'Tab::Room');
Tab::Panel->has_a(round => 'Tab::Round');
Tab::Panel->has_many(ballots => 'Tab::Ballot', 'panel');

__PACKAGE__->_register_dates( qw/timestamp/);

Tab::Panel->set_sql(prelims => qq{ 
							select __ESSENTIAL__ 
							from panel 
							where type="prelim" 
							and event = ? 
							order by "name"});

Tab::Panel->set_sql(by_site_and_tourn => "select distinct panel.* 
											from panel,round,event
											where round.site = ? 
											and panel.round = round.id
											and round.event = event.id
											and event.tourn = ?");

Tab::Panel->set_sql(conflicts_by_timeslot => "
							select distinct panel.id
							from entry as e1, entry as e2, 
							timeslot as ts1,timeslot as ts2,
							panel,ballot,round

							where e1.id = ?
							and ts1.id = ?

							and e2.id = ballot.entry
							and panel.id = ballot.panel
							and round.id = panel.round
							and ts2.id = round.timeslot

							and e2.id in 
								(select es2.id 
									from entry_student as es1, entry_student as es2
									where es1.student = es2.student
									and es1.entry = e1.id
									and es2.entry = e2.id
								)
							
							and e1.id != e2.id
							and ts1.start < ts2.end 
							and ts1.end > ts2.start
						");

Tab::Panel->set_sql(letters => qq{ select distinct letter 
		from panel where event = ? order by "letter"});

Tab::Panel->set_sql(by_judge => "select distinct panel.id 
								from panel,ballot 
								where ballot.judge= ? 
								and ballot.panel = panel.id " );

Tab::Panel->set_sql(preset_by_timeslot => "select distinct panel.id 
											from panel,round
											where panel.round = round.id
											and round.preset = 1
											and round.timeslot = ? ");

Tab::Panel->set_sql(by_nojudge => "select distinct panel.id 
				from panel,ballot,event,round
				where ballot.panel = panel.id 
				and panel.event = event.id 
				and panel.round = round.id
				and round.preset = 0
				and event.tourn = ? 
				and ballot.judge = 0");

Tab::Panel->set_sql(judgecount => "
				select panel.*, count(distinct judge.id) as num_judges
				from panel,event,ballot,judge
				where panel.event = event.id
				and panel.type = \"prelim\"
				and event.tourn = ?
				and ballot.panel = panel.id 
				and ballot.judge = judge.id
				group by panel");

Tab::Panel->set_sql(by_noroom => "select distinct panel.id 
				from panel,event where event.id = panel.event
				and event.tourn = ? and panel.room = 0");

Tab::Panel->set_sql(by_tourn => "select distinct panel.id 
				from panel,event 
				where event.tourn = ? 
				and panel.event = event.id");

Tab::Panel->set_sql(judge_and_timeslot => "select distinct panel.*
				from panel,ballot,round
				where ballot.judge= ? 
				and ballot.panel = panel.id 
				and panel.round = round.id
				and round.timeslot = ?" );

Tab::Panel->set_sql(by_event_and_timeslot => "select distinct panel.*
				from panel,round
				where panel.event = ? 
				and panel.round = round.id
				and round.timeslot = ? ");

Tab::Panel->set_sql(by_timeslot => "select distinct panel.id 
								from panel,round
								where panel.round = round.id
								and round.timeslot = ? ");

Tab::Panel->set_sql(roomless_by_timeslot => "select distinct panel.id 
								from panel,round
								where panel.round = round.id
								and panel.room < 1
								and round.timeslot = ? ");

Tab::Panel->set_sql(roomless_by_event_and_timeslot => "select distinct panel.id 
								from panel,round
								where panel.round = round.id
								and panel.room < 1
								and round.event = ? 
								and round.timeslot = ? ");

sub clear_empties {
	my $self = shift;
	my @empties = Tab::Ballot->search_empty_by_panel($self->id);

	foreach my $ballot (@empties) { 
		$ballot->delete;
	}

	return 1;
}

sub set_chair { 
	my ($self, $judge) = @_;
	Tab::Ballot->sql_remove_chairs->execute($self->id);
	Tab::Ballot->sql_add_chair->execute($self->id, $judge->id);
	return;
}

sub done { 
	my $self = shift;
	return if $self->round->preset;
	my @undone = Tab::Ballot->search_undone_by_panel($self->id);
	my $done = 1 unless (scalar @undone) > 0;
	return $done;
}

sub done_by_judge { 
	my ($self,$judge) = @_;
	return if $self->round->preset;
	my @undone = Tab::Ballot->search_undone_by_panel_and_judge($self->id, $judge->id);
	my $done = 1 unless (scalar @undone) > 0;
	return $done;
}

sub timeslot {
    my $self = shift;
    my $timeslot = $self->round->timeslot;
    return $timeslot;
}
sub judges {
    my $self = shift;
    my @judges = Tab::Judge->search_by_panel($self->id);
    return @judges;
}

sub entries {
    my $self = shift;
    return Tab::Entry->search_by_panel($self->id);
}

sub entries_region{
    my $self = shift;
    my @entries = Tab::Entry->search_by_panel_regcode($self->id);
    return @entries;
}

sub clean_judges{
	my ($self, $entry) = @_;
	foreach my $judge ($self->judges) { 
		return if $judge->cannot_judge($entry);
	}
	return 1;
}

Tab::Panel->set_sql( hits_score => "
   select count(distinct c1b1.entry,c2b1.entry,c1b1.panel) as hits_score
        from panel as p2,
        entry as c1,ballot as c1b1, ballot as c1b2,
        entry as c2,ballot as c2b1, ballot as c2b2
        where c2b2.entry = c1.id
        and c1b2.entry = c2.id
        and c2b2.panel = c1b2.panel
        and c2b2.panel =  ?
        and c1b1.entry = c1.id
        and c1b1.panel = p2.id
        and c2b1.entry = c2.id
        and c2b1.panel = p2.id
        and c2b2.panel != p2.id
   		and c1.dropped != 1
		and c2.dropped != 1
        and c1.code > c2.code
");

sub hits_score { 

	my $self = shift;
	return Tab::Panel->sql_hits_score->select_val($self->id);
}

Tab::Panel->set_sql( school_score => "
		select count(distinct b1.panel,b1.entry,b2.entry)
        from ballot as b1, entry as c1,
        ballot as b2, entry as c2

        where c2.school = c1.school
        and b1.panel = ?
        and b1.panel = b2.panel

        and b1.entry = c1.id
        and b2.entry = c2.id

   		and c1.dropped != 1
		and c2.dropped != 1

        and c1.id > c2.id
	");

sub school_score {
	my $self = shift;
	return Tab::Panel->sql_school_score->select_val($self->id);
}

Tab::Panel->set_sql( region_score => "
		select count(distinct b1.id)
        from ballot as b1, entry as c1, school as s1, 
        ballot as b2, entry as c2, school as s2

        where c1.school = s1.id
        and c2.school = s2.id

        and b1.entry = c1.id
        and b2.entry = c2.id

   		and c1.dropped != 1
		and c2.dropped != 1

		and s1.region = s2.region

        and b1.panel = ?
        and b1.panel = b2.panel

        and c1.id > c2.id
	");

sub region_score { 

	my $self = shift;
	return Tab::Panel->sql_region_score->select_val($self->id);

}

Tab::Panel->set_sql( entry_hits_score => "
   	select count(distinct panel.id) as hits_score
        from panel, ballot as c1b2,
        entry as c2,ballot as c2b1, ballot as c2b2

        where c2b1.entry = c2.id
        and c2b1.panel = ?

		and c2.dropped != 1

        and c1b2.entry = ?
        and c1b2.panel = c2b2.panel
        and c2b2.entry = c2.id

        and c2b2.panel != c2b1.panel
		and c2b2.panel = panel.id
        and c1b2.entry != c2.id
");

 Tab::Panel->set_sql( school_entry_hits_score => "
    select count(distinct panel.id) as hits_score
        from panel, ballot as c1b2,entry as c1,
        entry as c2,ballot as c2b1, ballot as c2b2
		
        where c2b1.entry = c2.id
        and c2b1.panel = ? 

        and c1.id = ? 
		and c2.dropped != 1
		and c1b2.entry = c1.id
        and c1b2.panel = c2b2.panel
        and c2b2.entry = c2.id

        and c2b2.panel != c2b1.panel
        and c2b2.panel = panel.id
        and c1.id != c2.id

		and c2.school = c1.school

    ");

sub entry_hits_score { 

	my ($self, $entry) = @_;

	my $score = Tab::Panel->sql_entry_hits_score->select_val($self->id, $entry->id);

	# Add in another point to penalize teammates hitting each other twice more.

	$score = $score + Tab::Panel->sql_school_entry_hits_score->select_val($self->id, $entry->id);

	return $score;
}

Tab::Panel->set_sql( entry_region_score => "
	select count(distinct c2.id) as region_score
		from entry as c1, entry as c2,
		ballot as b2,
		school as s1, school as s2

			where b2.panel = ?
			and c1.id = ?
			and c2.id = b2.entry
			
			and c1.dropped != 1
			and c2.dropped != 1
	
			and c1.school = s1.id
			and c2.school = s2.id

	        and s2.region = s1.region

			and c1.id != c2.id
    ");

sub entry_region_score { 
	my ($self, $entry) = @_;
	return Tab::Panel->sql_entry_region_score->select_val($self->id, $entry->id);
}

   	Tab::Panel->set_sql( entry_school_score => "
   	
		select count(distinct c2.id) as school_score
		from entry as c1, entry as c2, ballot as b2

			where b2.panel = ?
			and c1.id = ?
			and c1.dropped != 1
			and c2.dropped != 1
			and c2.id = b2.entry
			and c1.school = c2.school
			and c1.id != c2.id
	
    ");


sub entry_school_score { 

	my ($self, $entry) = @_;

	return Tab::Panel->sql_entry_school_score->select_val($self->id, $entry->id);

}

sub score { 

	my ($self, $entry) = @_;

	my $value;

	my $method = $entry->tourn->method;
	my $circuit = $entry->tourn->circuit;

	if	($entry) { 

	#School scoring

		$value = $self->entry_school_score($entry) * 10;

	#Second hit scoring

		$value = $value + ($self->entry_hits_score($entry) * 2);

	#Region scoring

		$value = $value + $self->entry_region_score($entry)  * 1
			if ($circuit->region_based);
		
		$value = $value + $self->entry_region_score($entry)  * 10
			if ($circuit->diocese_based);

		return $value;

	} else { 

	#School scoring
		
		$value = $self->school_score  * 10;

	#Second Hits

		$value = $value + $self->hits_score  * 2;

	#Region score
		
		$value = $value + $self->region_score * 1
			if ($circuit->region_based);

		$value = $value + $self->region_score * 10
			if ($circuit->diocese_based);
		
		return $value;

	}

}

sub update_score { 

	my ($self, $entry) = @_;

	my $value;

	my $method = $entry->tourn->method;
	my $circuit = $entry->tourn->circuit;

	if	($entry) { 

	#School scoring

		$value = $self->entry_school_score($entry) * 10;

	#Second hit scoring

		$value = $value + $self->entry_hits_score($entry) * 2;

	#Region scoring

	   $value = $value + $self->entry_region_score($entry) * 1
			if ($circuit->diocese_based || $circuit->region_based);
		
	} else { 

	#School scoring
		
		$value = $self->school_score * 10;

	#Second Hits

		$value = $value + $self->hits_score * 2;

	#Region score
		
		$value = $value + $self->region_score * 1
			if ($circuit->diocese_based || $circuit->region_based);
		
	}

		$self->tmp_score($value);
		$self->update;
		return;
}

sub entries_in_order {

	my $self = shift;
	return Tab::Entry->search_by_panel_rank($self->id);

}
