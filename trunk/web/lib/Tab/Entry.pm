package Tab::Entry;
use base 'Tab::DBI';
Tab::Entry->table('entry');
Tab::Entry->columns(Primary => qw/id/);
Tab::Entry->columns(Essential => qw/code dropped name school tourn event/);
Tab::Entry->columns(Others => qw/seed bid title ada waitlist dq drop_time reg_time timestamp/);

Tab::Entry->columns(TEMP => qw/last_round rank rank_in_round speaks letter etype 
					schcode regcode schoolid regionid panelid points ranks recips points/);

Tab::Entry->has_a(school => 'Tab::School');
Tab::Entry->has_a(tourn => 'Tab::Tourn');
Tab::Entry->has_a(event => 'Tab::Event');
Tab::Entry->has_many(ballots => 'Tab::Ballot', 'entry');
Tab::Entry->has_many(changes => 'Tab::TournChange', 'entry');
Tab::Entry->has_many(ratings => 'Tab::Rating', 'entry');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/drop_time/);
__PACKAGE__->_register_datetimes( qw/reg_time/);

Tab::Entry->set_sql(wipe_prefs => " delete from rating where entry = ?");

Tab::Entry->set_sql(prelim_cume_by_event => " 
					select distinct entry.id as id, entry.code as code,
						SUM(ballot.rank) as prelim_cume
						from entry,ballot,panel       
						where ballot.panel = panel.id   
						and panel.type = \"prelim\"       
						and ballot.entry = entry.id
						and entry.event = ?
						group by entry");

Tab::Entry->set_sql(sweep_prelims => " 
				select distinct entry.id as id,entry.code as code,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from entry,ballot,panel,event
                where panel.type = \"prelim\"
                and panel.id = ballot.panel
                and ballot.entry = entry.id
                and entry.dropped != 1
                and entry.event = event.id
                and event.type = \"speech\"
                and entry.tourn = ?
                and ballot.rank != 0
                group by entry");

Tab::Entry->set_sql(sweep_elims => " 
				select distinct entry.id as id,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from entry,ballot,panel,event
                where panel.type = \"elim\"
                and panel.id = ballot.panel
				and panel.nosweep != 1
                and ballot.entry = entry.id
                and entry.dropped != 1
                and entry.event = event.id
                and event.type = \"speech\"
                and entry.tourn = ?
                and ballot.rank != 0
                group by entry");

Tab::Entry->set_sql(sweep_finals => " 
				select distinct entry.id as id,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from entry,ballot,panel,event
                where panel.type = \"final\"
                and panel.id = ballot.panel
                and ballot.entry = entry.id
                and entry.dropped != 1
                and entry.event = event.id
                and event.type = \"speech\"
                and entry.tourn = ?
                and ballot.rank != 0
                group by entry");

Tab::Entry->set_sql(ties => "select distinct c1.id as me, c2.id 
									from entry as c1, entry as c2
									where c1.id = ? 
									and c1.event = c2.event
									and c2.dropped != 1
									and c2.dq != 1
									and c1.id != c2.id
									and c1.tb0 = c2.tb0
									and c1.tb0 = c2.tb0
									and c1.tb1 = c2.tb1
									and c1.tb2 = c2.tb2
									and c1.tb3 = c2.tb3
									and c1.tb4 = c2.tb4
									and c1.tb5 = c2.tb5
									and c1.tb6 = c2.tb6
									and c1.tb7 = c2.tb7
									and c1.tb8 = c2.tb8
									and c1.tb9 = c2.tb9");

Tab::Entry->set_sql(disrating_tierified => "select distinct entry.* 
						from entry,event
						where event.tourn = ? 
						and entry.event = event.id
						and entry.dq = 1");

Tab::Entry->set_sql(count_active_by_event => "select count(distinct entry.id)
                       from entry
                       where entry.event = ? 
                       and entry.dropped != 1
                       and entry.waitlist != 1");

Tab::Entry->set_sql(count_waitlist_by_event => "select count(distinct entry.id)
                       from entry
                       where entry.event = ? 
                       and entry.dropped != 1
                       and entry.waitlist = 1");

Tab::Entry->set_sql(by_event_order_and_timeslot => "
					select distinct entry.*,panel.letter as letter
					from entry,ballot,panel,round
					where entry.event = ? 
					and entry.id = ballot.entry
					and ballot.speakerorder = ? 
					and ballot.panel = panel.id
					and panel.round = round.id
					and round.timeslot = ? 
					order by panel.letter");

Tab::Entry->set_sql(team_members => qq{ 
				select distinct entry.id
				from team_member,entry
				where team_member.student= ?
				and team_member.entry = entry.id
				and entry.tourn = ? 
		});

Tab::Entry->set_sql(order_total => "select sum(ballot.speakerorder) 
		from ballot,panel
		where ballot.panel = panel.id
		and panel.type = \"prelim\"
		and ballot.entry =  ?");

Tab::Ballot->set_sql(noshows_hit => "select count(distinct ballot.id) 
							from ballot, ballot as b2
							where ballot.panel = b2.panel
							and b2.entry = ? 
							and ballot.entry != b2.entry
							and ballot.speakerorder < b2.speakerorder
							and ballot.noshow = 1");

Tab::Ballot->set_sql(drops_hit => "select count(distinct ballot.id) 
							from ballot, ballot as b2, entry
							where ballot.panel = b2.panel
							and b2.entry = ? 
							and ballot.entry != b2.entry
							and ballot.entry = entry.id
							and entry.dropped = 1
							and ballot.speakerorder < b2.speakerorder");

Tab::Entry->set_sql(speech_entrys_by_tourn => "select distinct entry.*
							from entry,event
							where entry.event = event.id
							and event.tourn = ?
							and event.type = \"speech\"");

Tab::Entry->set_sql(debate_congress_entrys_by_tourn => "select distinct entry.*
							from entry,event
							where entry.event = event.id
							and event.tourn = ?
							and event.type != \"speech\"");

sub side { 

	my ($self, $round) = @_;
	
	Tab::Ballot->set_sql(side_by_round => "select ballot.side from ballot,panel
							where ballot.panel = panel.id
							and ballot.entry = ?
							and panel.round = ?" );

	return Tab::Ballot->sql_side_by_round->select_val($self->id,$round->id);
}


sub strikes { 

	my ($self) = @_;

	my $tourn =  $self->school->tourn;

	my @strikes = Tab::Strike->search(	
					entry => $self->id,
					strike => 1 );

	return @strikes;

}


sub team_name {

	my $self = shift;

	my @students = $self->students;

	if (scalar @students == 1) { 
		my $student = shift @students;
		return $student->first." ".$student->last;
	}

	my $name;
	my $not_first;

	foreach my $student (@students) { 
		$name .= " & " if $not_first;
		$name .= $student->last;
		$not_first++:
	}

	return $name;

}

sub full_team_name {

	my $self = shift;

	my @students = $self->students;

	if (scalar @students == 1) { 
		my $student = shift @students;
		return $student->first." ".$student->last;
	}

	my $name;
	my $not_first;

	foreach my $student (@students) { 
		$name .= " & " if $not_first;
		$name .= $student->first." & ".$student->last;
		$not_first++:
	}

	return $name;

}

sub latest_round {
	my $self = shift;
	my @rounds = Tab::Round->search_latest_by_entry($self->id);
	return shift @rounds;
}

sub ncfl_speaks_check {

	my $self = shift;
	my $status;
	my $early;
	my $mid;
	my $late;

	foreach my $round ($self->event->rounds( type => "prelim")) { 

		my $order = $self->order_by_round($round);

		$early++ if $order < 3;
		$mid++ if $order > 2 && $order < 6;
		$late++ if $order > 5;
		
	}

	my $status = "OK" if $early && $mid && $late;
	$status = "NOT OK" unless $status;
	return ($status, $early, $mid, $late);

}

sub order_total {
	my $self = shift;
	my $total = Tab::Entry->sql_order_total->select_val($self->id);
	my $adjust_noshows = Tab::Ballot->sql_noshows_hit->select_val($self->id);
	my $adjust_drops = Tab::Ballot->sql_drops_hit->select_val($self->id);
	$total = $total - $adjust_noshows;
	$total = $total - $adjust_drops;
	$total = $total/3;
	return $total;
}

Tab::Entry->set_sql(highest_code => "select MAX(code) from entry where event = ?");

Tab::Entry->set_sql(lowest_code => "select MIN(code) from entry where event = ?");

Tab::Entry->set_sql(mult_last_name => "
					select distinct entry.* from entry,school
					where entry.name like ?
					and entry.school = school.id
					and school.tourn = ?");

Tab::Entry->set_sql(part_last_name => "
					select distinct entry.* from entry,student,school
						where entry.partner = student.id
						and student.last like ?
						and entry.school = school.id
						and school.tourn = ?");

Tab::Entry->set_sql(stud_last_name => "
					select distinct entry.* from entry,student,school
						where entry.student = student.id
						and student.last like ?
						and entry.school = school.id
						and school.tourn = ?");

Tab::Entry->set_sql(team_last_name => " 
					select distinct entry.* from entry,student,team_member,school
						where entry.id = team_member.entry
						and team_member.student = student.id
						and student.last like ? 
						and entry.school = school.id
						and school.tourn = ?");

Tab::Entry->set_sql(event_last_name => "
					select distinct entry.* from entry,student
					where entry.student = student.id
					and entry.event = ?
					and student.last = ?
					");

Tab::Entry->set_sql(school_event_last_name => "
					select distinct entry.* from entry,student
					where entry.student = student.id
					and entry.school = ?
					and entry.event = ?
					and student.last = ?
					");

Tab::Entry->set_sql( not_mine => "
				select distinct entry.* from entry,panel,ballot
                   where (select count(distinct b2.entry)
                   from ballot as b2 where b2.panel = panel.id) = ?
                   and entry.dropped != 1
                   and entry.dq != 1
                   and ballot.entry = entry.id
                   and ballot.panel = panel.id
                   and panel.round = ?
                     order by entry.code
                    ");

sub panel_in_round {
	my ($self,$round) = @_;
	my @panels = Tab::Panel->search_by_entry_in_round($self->id,$round->id);
	return shift @panels;
}

Tab::Entry->set_sql(by_region_and_pool => "
						select distinct entry.*
						from entry,school,pool,round
						where entry.school = school.id
						and entry.event = round.event
						and school.region = ? 
						and round.pool = ?");

Tab::Entry->set_sql(by_region_and_event => "
						select distinct entry.id from entry,student,school
						where school.region = ? 
						and entry.school = school.id
                		and entry.event = ?
				");

Tab::Entry->set_sql(active_by_region_and_event => "
				select distinct entry.id from entry,school
                where school.region = ?
				and school.tourn = ? 
				and school.id = entry.school
                and entry.event = ?
				and entry.dropped != 1
				and entry.waitlist != 1
				");

Tab::Entry->set_sql(active_by_region => "
				select distinct entry.id from entry,school
                where school.region = ?
				and school.tourn = ? 
				and school.id = entry.school
				and entry.dropped != 1
				and entry.waitlist != 1
				");

Tab::Entry->set_sql(by_region_and_group => "
				select distinct entry.id from entry,school,event
                where school.region = ?
				and school.id = entry.school
                and entry.event = event.id
				and event.judge_group = ? 
				");

Tab::Entry->set_sql(by_group_school=> qq{select distinct entry.* 
								from entry,event
								where entry.event = event.id
								and entry.school = ?
								and entry.waitlist != 1
								and entry.dropped != 1
								and event.no_judge_burden = 0
								and event.judge_group = ?});

Tab::Entry->set_sql(by_group=>  qq{select distinct entry.* 
								from entry,event
								where entry.event = event.id
								and entry.waitlist != 1
								and entry.dropped != 1
								and event.judge_group = ?});			

Tab::Entry->set_sql(by_round=> qq{select distinct entry.*,panel.id as panelid
								from entry,panel,ballot
								where entry.id = ballot.entry
								and entry.dropped != 1
								and entry.dq != 1
								and ballot.panel = panel.id
								and panel.round = ?
								order by ballot.speakerorder});



Tab::Entry->set_sql( by_panel_rank => "select entry.*,sum(ballot.rank) as ranks,
								sum(1/ballot.rank) as recips,sum(points) as points
								from entry,ballot
								where ballot.entry = entry.id 
								and ballot.panel = ?
								group by entry
								order by ranks, recips DESC,points DESC");

Tab::Entry->set_sql(by_panel=> qq{ 
			select distinct entry.id,entry.code,school.code as schcode,
			ballot.speakerorder as speaks,
			school.id as schoolid,school.region as regionid
			from entry,ballot,school
			where entry.dropped != 1
			and entry.dq != 1
			and ballot.panel = ? 	
			and ballot.entry = entry.id 
			and entry.school = school.id
			order by ballot.speakerorder,entry.id});

Tab::Entry->set_sql(by_panel_regcode => qq{ 
				select distinct entry.*,region.code as regcode
				from entry,ballot,school,region

				where entry.dropped = 0 
				and ballot.panel = ? 	
				and ballot.entry = entry.id 

				and entry.school = school.id
				and school.region = region.id
				order by ballot.speakerorder,entry.id});

Tab::Entry->set_sql(has_judged => "  select distinct entry.id 
									from entry,ballot 
									where ballot.judge = ? 
									and ballot.entry = entry.id");

sub region { 
	my $self = shift;
	return $self->school->region;
}

sub how_doubled { 
	my ($self, $panel) = @_;
	return if $self->event->team == 3;
	my @others = Tab::Panel->search_conflicts( 
			$self->student->id, 
			$panel->round->timeslot->id, 
			$self->id);

	push (@others,Tab::Panel->search_partner_conflicts( 
			$self->student->id, 
			$panel->round->timeslot->id, 
			$self->id));

	return @others if @others;
	return;
}

sub is_conflicted  {

    my ($self, $judge) = @_;

    my @strikes = Tab::Strike->search(
                    entry => $self->id,
                    judge => $judge->id,
                    strike => 0 );

    my $strike = shift @strikes;

    foreach my $spare (@strikes) {
        $spare->delete;
    }

    return $strike;
}

sub conflicts { 

	my ($self, $only) = @_;
	my $tourn = $self->school->tourn;
	my $group = $self->event->judge_group;

	my @strikes = Tab::Strike->search( entry => $self->id, strike => 0 );


	if ($only) { 

    	my %seen = ();  #uniq
		@strikes = grep { ! $seen{$_->judge->id} ++ } @strikes;
		return @strikes;

	} else {

		my @school_strikes = Tab::Strike->search( school => $self->school->id, strike => 0 );
    	my @filtered_strikes;

    	foreach $strike (@school_strikes) {
        	next unless $strike->judge->judge_group->id == $group->id;
        	push (@filtered_strikes, $strike);
    	}
	
		push (@strikes, @filtered_strikes);
    	my %seen = ();  #uniq
		@strikes = grep { ! $seen{$_->judge->id} ++ } @strikes;
		return @strikes;
	}


}

sub has_judged {
    my $self = shift;
    my @students = Tab::Judge->search_has_judged($self->id);
    return @students;
}

sub finals { 
	my $self = shift;
	return Tab::Round->search_finals_by_entry($self->id);
}

sub elims { 
	my $self = shift;
	return Tab::Round->search_elims_by_entry($self->id);
}


sub add_member { 
	my ($self, $student) = @_;
	my @existing = Tab::TeamMember->search( student => $student->id, entry => $entry->id );
	Tab::TeamMember->create({ student => $student->id, entry => $entry->id }) unless @existing;
	return;
}

sub rm_member { 
	my ($self, $student) = @_;
	my @existing = Tab::TeamMember->search( student => $student->id, entry => $entry->id );
	foreach (@existing) { $_->delete; }
	return;
}


sub members {
    my $self = shift;
	my @students;
   	push (@students, Tab::Student->search_team_members($self->id)) if $self->event->team == 3;
	push (@students, $self->student) if $self->student->id > 0;
	push (@students, $self->partner) if $self->partner && $self->partner->id > 0;
    return @students;
}

sub has_seen {
    my $self = shift;
    my @judges = Tab::Judge->search_has_seen($self->id);
    return @judges;
}


sub panels {
    my $self = shift;
    return Tab::Panel->search_entry_panels($self->id);
}

sub prelims {
    my $self = shift;
    return Tab::Panel->search_entry_prelims($self->id);
}

sub round_panel {
    my ($self,$round) = @_;
    my @panels = Tab::Panel->search_entry_panels_by_round($self->id, $round->id);
	my $one = shift @panels;
	return $one;
}


sub hits {
    my $self;
    my $other;
    ($self, $other) = @_ ;
    return if $self->id == $other->id;
    my @common_panels;
    my @self_panels = $self->panels;
    my @other_panels = $other->panels;
    my (@isect,@diff,@union);
    @isect =  @diff =  @union = ();
    my %count;
    foreach $e (@self_panels, @other_panels) { $count{$e}++ }
    foreach $e (keys %count) {
        push(@union, $e);
        push @{ $count{$e} == 2 ? \@isect : \@diff }, $e;
    }
    return (@isect);
}

sub seed { 
	my ($self,$panel) = @_;
	my @ballots = Tab::Ballot->search( 
		entry => $self->id, 
		panel => $panel->id, 
		{order_by => "seed DESC"});
	return $ballots[0]->seed if @ballots;
	return;
} 

sub setorder { 
	my ($self, $order, $panel_id) = @_;
	my $sql_string = "update ballot
		set ballot.speakerorder = $order 
		where ballot.entry = ".$self->id."
		and ballot.panel = $panel_id";
	Tab::Entry->set_sql(set_custom_order => $sql_string);
	Tab::Entry->sql_set_custom_order->execute;
}

sub set_question_timeslot {
	my ($self, $timeslot, $question) = @_;
	Tab::Ballot->sql_set_question_by_timeslot->execute($question, $self->id, $timeslot->id);
	return;
}

sub question_by_timeslot {
	my ($self, $timeslot, $question) = @_;
	return Tab::Ballot->sql_question_by_timeslot->select_val($self->id, $timeslot->id);
}

sub order_by_round { 
	my ($self, $round) = @_;
	my $sql_string = "select distinct ballot.speakerorder
			from ballot,panel,round 
			where ballot.entry = ".$self->id."
			and ballot.panel = panel.id
			and panel.round = ".$round->id;
	Tab::Entry->set_sql(order_by_round => $sql_string);
	return Tab::Entry->sql_order_by_round->select_val;
}

sub speakerorder { 
	my ($self,$panel) = @_;
	my @ballots = Tab::Ballot->search( 
		entry => $self->id, 
		panel => $panel->id, 
		{order_by => "speakerorder DESC"});
	return $ballots[0]->speakerorder if @ballots;
	return;
}

sub cume { 

	my $self = shift;
	Tab::Entry->set_sql(overall_cume => "select SUM(ballot.rank) from ballot
						where ballot.entry = ".$self->id);

	return Tab::Entry->sql_overall_cume->select_val;
}

Tab::Entry->set_sql(prelim_cume => "select SUM(ballot.rank) from ballot,panel       
								where ballot.panel = panel.id   
								and panel.type = \"prelim\"       
								and ballot.entry = ?");
sub prelim_cume { 
	my $self = shift;
	return Tab::Entry->sql_prelim_cume->select_val($self->id);
}

sub prelim_ballots { 
	my $self = shift;
	return Tab::Ballot->search_from_prelims($self->id);
}

sub elim_ballots { 
	my $self = shift;
	return Tab::Ballot->search_from_elims($self->id);
}

sub final_ballots { 
	my $self = shift;
	return Tab::Ballot->search_from_finals($self->id);
}



Tab::Entry->set_sql(finals_by_school => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.school = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Entry->set_sql(elims_by_school => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.school = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Entry->set_sql(finals_by_event => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.event = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Entry->set_sql(elims_by_event => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.event = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Entry->set_sql(active_by_event => "select distinct entry.*
									from entry
									where entry.event = ?
									and entry.dropped != 1
									and entry.waitlist != 1");

Tab::Entry->set_sql(waitlist_by_event => "select distinct entry.*
									from entry
									where entry.event = ?
									and entry.dropped != 1
									and entry.waitlist = 1");

Tab::Entry->set_sql(finals_by_tourn => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.tourn = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Entry->set_sql(elims_by_tourn => "select distinct entry.id
									from entry,ballot,panel
									where entry.id = ballot.entry
									and entry.tourn = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Entry->set_sql(by_region => "select distinct entry.id 
									from entry,school
									where school.region = ?  
									and school.tourn = ? 
									and school.id = entry.school" );

Tab::Entry->set_sql(double_entries => "select distinct c2.id
        							from entry as c2, entry as c1, event
    							    where c2.student = c1.student
    							    and c2.tourn = c1.tourn
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Entry->set_sql(double_partners => "select distinct c2.id
        							from entry as c2, entry as c1, event
    							    where c2.partner = c1.student
    							    and c2.tourn = c1.tourn
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Entry->set_sql(double_partners_twice => "select distinct c2.id
        							from entry as c2, entry as c1, event
    							    where c1.partner is not null
									and c1.partner != 0
									and c2.partner = c1.partner
    							    and c2.tourn = c1.tourn
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Entry->set_sql(tourn_teams => "select distinct entry.* from entry,team_member
                                    where team_member.student = ? and
                                    team_member.entry = entry.id and
                                    tourn = ?");

Tab::Entry->set_sql(by_tourn_and_code => "select distinct entry.* from entry,tourn,event
									where entry.event = event.id
									and event.tourn = ?
									and entry.code = ?");

Tab::Entry->set_sql(by_timeslot => " select distinct entry.* 
									from entry,ballot,round,panel
		    						where entry.id = ballot.entry
   									and ballot.panel = panel.id
    								and panel.round = round.id
    								and round.timeslot = ? ");

Tab::Entry->set_sql(by_timeslot_and_event => " select distinct entry.* 
									from entry,ballot,round,panel
		    						where entry.id = ballot.entry
   									and ballot.panel = panel.id
    								and panel.round = round.id
    								and round.timeslot = ?
									and panel.event = ? ");

Tab::Entry->set_sql(housed => "select distinct entry.*
									from entry, housing
									where entry.school = ? 
									and entry.tourn = housing.tourn
									and entry.student = housing.student");

Tab::Entry->set_sql(tied => "select distinct c1.id, c2.id 
									from entry as c1, entry as c2,
									ballot as b1, ballot as b2
									where c1.id != c2.id
									and c1.id = b1.entry
									and c2.id = b2.entry
									and b1.panel = ?
									and b1.panel = b2.panel
									and c1.tb0 = c2.tb0
									and c1.tb0 = c2.tb0
									and c1.tb1 = c2.tb1
									and c1.tb2 = c2.tb2
									and c1.tb3 = c2.tb3
									and c1.tb4 = c2.tb4
									and c1.tb5 = c2.tb5
									and c1.tb6 = c2.tb6
									and c1.tb7 = c2.tb7
									and c1.tb8 = c2.tb8
									and c1.tb9 = c2.tb9");

Tab::Entry->set_sql(unballoted_entrys_by_tourn => "select distinct entry.*
									from entry,event
									where entry.tourn = ? 
									and entry.event = event.id
									and event.type != \"debate\"
									and entry.dropped != 1
									and entry.waitlist != 1
									and not exists (
										select id from ballot where entry = entry.id )
									");


