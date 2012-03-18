package Tab::Comp;
use base 'Tab::DBI';
Tab::Comp->table('comp');
Tab::Comp->columns(Primary => qw/id/);
Tab::Comp->columns(Essential => qw/student code dropped waitlist name school event partner dq/);

Tab::Comp->columns(Others => qw/apda_seed sweeps_points results_bar 
					bid title ada notes timestamp tournament 
					tb0 tb1 tb2 tb3 tb4 tb5 tb6 tb7 tb8 tb9 
					dropped_at registered_at
					qualifier qual2 qualexp qual2exp /);

Tab::Comp->columns(TEMP => qw/last_round rank rank_in_round speaks letter etype 
					schcode regcode schoolid regionid panelid points ranks recips points/);

Tab::Comp->has_a(student => 'Tab::Student');
Tab::Comp->has_a(partner => 'Tab::Student');
Tab::Comp->has_a(school => 'Tab::School');
Tab::Comp->has_a(tournament => 'Tab::Tournament');
Tab::Comp->has_a(event => 'Tab::Event');
Tab::Comp->has_many(ballots => 'Tab::Ballot', 'comp');
Tab::Comp->has_many(changes => 'Tab::Change', 'comp');
Tab::Comp->has_many(ratings => 'Tab::Rating', 'comp');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/dropped_at/);
__PACKAGE__->_register_datetimes( qw/registered_at/);

Tab::Comp->set_sql(wipe_prefs => " delete from rating where comp = ?");

Tab::Comp->set_sql(prelim_cume_by_event => " 
					select distinct comp.id as id, comp.code as code,
						SUM(ballot.rank) as prelim_cume
						from comp,ballot,panel       
						where ballot.panel = panel.id   
						and panel.type = \"prelim\"       
						and ballot.comp = comp.id
						and comp.event = ?
						group by comp");

Tab::Comp->set_sql(sweep_prelims => " 
				select distinct comp.id as id,comp.code as code,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from comp,ballot,panel,event
                where panel.type = \"prelim\"
                and panel.id = ballot.panel
                and ballot.comp = comp.id
                and comp.dropped != 1
                and comp.event = event.id
                and event.type = \"speech\"
                and comp.tournament = ?
                and ballot.rank != 0
                group by comp");

Tab::Comp->set_sql(sweep_elims => " 
				select distinct comp.id as id,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from comp,ballot,panel,event
                where panel.type = \"elim\"
                and panel.id = ballot.panel
				and panel.nosweep != 1
                and ballot.comp = comp.id
                and comp.dropped != 1
                and comp.event = event.id
                and event.type = \"speech\"
                and comp.tournament = ?
                and ballot.rank != 0
                group by comp");

Tab::Comp->set_sql(sweep_finals => " 
				select distinct comp.id as id,
					sum(IF(ballot.rank < 5,6 - ballot.rank,1)) as points
                from comp,ballot,panel,event
                where panel.type = \"final\"
                and panel.id = ballot.panel
                and ballot.comp = comp.id
                and comp.dropped != 1
                and comp.event = event.id
                and event.type = \"speech\"
                and comp.tournament = ?
                and ballot.rank != 0
                group by comp");

Tab::Comp->set_sql(ties => "select distinct c1.id as me, c2.id 
									from comp as c1, comp as c2
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

Tab::Comp->set_sql(disqualified => "select distinct comp.* 
						from comp,event
						where event.tournament = ? 
						and comp.event = event.id
						and comp.dq = 1");

Tab::Comp->set_sql(count_active_by_event => "select count(distinct comp.id)
                       from comp
                       where comp.event = ? 
                       and comp.dropped != 1
                       and comp.waitlist != 1");

Tab::Comp->set_sql(count_waitlist_by_event => "select count(distinct comp.id)
                       from comp
                       where comp.event = ? 
                       and comp.dropped != 1
                       and comp.waitlist = 1");

Tab::Comp->set_sql(by_event_order_and_timeslot => "
					select distinct comp.*,panel.letter as letter
					from comp,ballot,panel,round
					where comp.event = ? 
					and comp.id = ballot.comp
					and ballot.speakerorder = ? 
					and ballot.panel = panel.id
					and panel.round = round.id
					and round.timeslot = ? 
					order by panel.letter");

Tab::Comp->set_sql(team_members => qq{ 
				select distinct comp.id
				from team_member,comp
				where team_member.student= ?
				and team_member.comp = comp.id
				and comp.tournament = ? 
		});

Tab::Comp->set_sql(order_total => "select sum(ballot.speakerorder) 
		from ballot,panel
		where ballot.panel = panel.id
		and panel.type = \"prelim\"
		and ballot.comp =  ?");

Tab::Ballot->set_sql(noshows_hit => "select count(distinct ballot.id) 
							from ballot, ballot as b2
							where ballot.panel = b2.panel
							and b2.comp = ? 
							and ballot.comp != b2.comp
							and ballot.speakerorder < b2.speakerorder
							and ballot.noshow = 1");

Tab::Ballot->set_sql(drops_hit => "select count(distinct ballot.id) 
							from ballot, ballot as b2, comp
							where ballot.panel = b2.panel
							and b2.comp = ? 
							and ballot.comp != b2.comp
							and ballot.comp = comp.id
							and comp.dropped = 1
							and ballot.speakerorder < b2.speakerorder");

Tab::Comp->set_sql(speech_comps_by_tourn => "select distinct comp.*
							from comp,event
							where comp.event = event.id
							and event.tournament = ?
							and event.type = \"speech\"");

Tab::Comp->set_sql(debate_congress_comps_by_tourn => "select distinct comp.*
							from comp,event
							where comp.event = event.id
							and event.tournament = ?
							and event.type != \"speech\"");

sub side { 

	my ($self, $round) = @_;
	
	Tab::Ballot->set_sql(side_by_round => "select ballot.side from ballot,panel
							where ballot.panel = panel.id
							and ballot.comp = ?
							and panel.round = ?" );

	return Tab::Ballot->sql_side_by_round->select_val($self->id,$round->id);
}


sub strikes { 

	my ($self) = @_;

	my $tourn =  $self->school->tournament;

	my @strikes = Tab::Strike->search(	
					comp => $self->id,
					strike => 1 );

	return @strikes;

}


sub team_name {

	my $self = shift;

	my $name = $self->student->first." ".$self->student->last if $self->event->team == 1;

	if ($self->event->team == 2) { 

		$name = $self->student->last if $self->student;
		$name .= "* " if $self->student && $self->student->novice;
	
		$name .= " & ".$self->partner->last if $self->partner;
		$name .= "* " if $self->partner && $self->partner->novice;

	}

	$name = $self->name if $self->event->team == 3;

	return $name;

}

sub full_team_name {

	my $self = shift;

	my $name = $self->student->first." ".$self->student->last unless $self->event->team == 3;
	$name .= " & ".$self->partner->first." ".$self->partner->last if $self->partner && $self->event->team < 3;
	$name = $self->name if $self->event->team == 3;

	return $name;

}

sub latest_round {
	my $self = shift;
	my @rounds = Tab::Round->search_latest_by_comp($self->id);
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
	my $total = Tab::Comp->sql_order_total->select_val($self->id);
	my $adjust_noshows = Tab::Ballot->sql_noshows_hit->select_val($self->id);
	my $adjust_drops = Tab::Ballot->sql_drops_hit->select_val($self->id);
	$total = $total - $adjust_noshows;
	$total = $total - $adjust_drops;
	$total = $total/3;
	return $total;
}

Tab::Comp->set_sql(highest_code => "select MAX(code) from comp where event = ?");

Tab::Comp->set_sql(lowest_code => "select MIN(code) from comp where event = ?");

Tab::Comp->set_sql(mult_last_name => "
					select distinct comp.* from comp,school
					where comp.name like ?
					and comp.school = school.id
					and school.tournament = ?");

Tab::Comp->set_sql(part_last_name => "
					select distinct comp.* from comp,student,school
						where comp.partner = student.id
						and student.last like ?
						and comp.school = school.id
						and school.tournament = ?");

Tab::Comp->set_sql(stud_last_name => "
					select distinct comp.* from comp,student,school
						where comp.student = student.id
						and student.last like ?
						and comp.school = school.id
						and school.tournament = ?");

Tab::Comp->set_sql(team_last_name => " 
					select distinct comp.* from comp,student,team_member,school
						where comp.id = team_member.comp
						and team_member.student = student.id
						and student.last like ? 
						and comp.school = school.id
						and school.tournament = ?");

Tab::Comp->set_sql(event_last_name => "
					select distinct comp.* from comp,student
					where comp.student = student.id
					and comp.event = ?
					and student.last = ?
					");

Tab::Comp->set_sql(school_event_last_name => "
					select distinct comp.* from comp,student
					where comp.student = student.id
					and comp.school = ?
					and comp.event = ?
					and student.last = ?
					");

Tab::Comp->set_sql( not_mine => "
				select distinct comp.* from comp,panel,ballot
                   where (select count(distinct b2.comp)
                   from ballot as b2 where b2.panel = panel.id) = ?
                   and comp.dropped != 1
                   and comp.dq != 1
                   and ballot.comp = comp.id
                   and ballot.panel = panel.id
                   and panel.round = ?
                     order by comp.code
                    ");

sub panel_in_round {
	my ($self,$round) = @_;
	my @panels = Tab::Panel->search_by_comp_in_round($self->id,$round->id);
	return shift @panels;
}

Tab::Comp->set_sql(by_region_and_pool => "
						select distinct comp.*
						from comp,school,pool,round
						where comp.school = school.id
						and comp.event = round.event
						and school.region = ? 
						and round.pool = ?");

Tab::Comp->set_sql(by_region_and_event => "
						select distinct comp.id from comp,student,school
						where school.region = ? 
						and comp.school = school.id
                		and comp.event = ?
				");

Tab::Comp->set_sql(active_by_region_and_event => "
				select distinct comp.id from comp,school
                where school.region = ?
				and school.tournament = ? 
				and school.id = comp.school
                and comp.event = ?
				and comp.dropped != 1
				and comp.waitlist != 1
				");

Tab::Comp->set_sql(active_by_region => "
				select distinct comp.id from comp,school
                where school.region = ?
				and school.tournament = ? 
				and school.id = comp.school
				and comp.dropped != 1
				and comp.waitlist != 1
				");

Tab::Comp->set_sql(by_region_and_group => "
				select distinct comp.id from comp,school,event
                where school.region = ?
				and school.id = comp.school
                and comp.event = event.id
				and event.judge_group = ? 
				");

Tab::Comp->set_sql(by_group_school=> qq{select distinct comp.* 
								from comp,event
								where comp.event = event.id
								and comp.school = ?
								and comp.waitlist != 1
								and comp.dropped != 1
								and event.no_judge_burden = 0
								and event.judge_group = ?});

Tab::Comp->set_sql(by_group=>  qq{select distinct comp.* 
								from comp,event
								where comp.event = event.id
								and comp.waitlist != 1
								and comp.dropped != 1
								and event.judge_group = ?});			

Tab::Comp->set_sql(by_round=> qq{select distinct comp.*,panel.id as panelid
								from comp,panel,ballot
								where comp.id = ballot.comp
								and comp.dropped != 1
								and comp.dq != 1
								and ballot.panel = panel.id
								and panel.round = ?
								order by ballot.speakerorder});



Tab::Comp->set_sql( by_panel_rank => "select comp.*,sum(ballot.rank) as ranks,
								sum(1/ballot.rank) as recips,sum(points) as points
								from comp,ballot
								where ballot.comp = comp.id 
								and ballot.panel = ?
								group by comp
								order by ranks, recips DESC,points DESC");

Tab::Comp->set_sql(by_panel=> qq{ 
			select distinct comp.id,comp.code,school.code as schcode,
			ballot.speakerorder as speaks,
			school.id as schoolid,school.region as regionid
			from comp,ballot,school
			where comp.dropped != 1
			and comp.dq != 1
			and ballot.panel = ? 	
			and ballot.comp = comp.id 
			and comp.school = school.id
			order by ballot.speakerorder,comp.id});

Tab::Comp->set_sql(by_panel_regcode => qq{ 
				select distinct comp.*,region.code as regcode
				from comp,ballot,school,region

				where comp.dropped = 0 
				and ballot.panel = ? 	
				and ballot.comp = comp.id 

				and comp.school = school.id
				and school.region = region.id
				order by ballot.speakerorder,comp.id});

Tab::Comp->set_sql(has_judged => "  select distinct comp.id 
									from comp,ballot 
									where ballot.judge = ? 
									and ballot.comp = comp.id");

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
                    comp => $self->id,
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
	my $tourn = $self->school->tournament;
	my $group = $self->event->judge_group;

	my @strikes = Tab::Strike->search( comp => $self->id, strike => 0 );


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
	return Tab::Round->search_finals_by_comp($self->id);
}

sub elims { 
	my $self = shift;
	return Tab::Round->search_elims_by_comp($self->id);
}


sub add_member { 
	my ($self, $student) = @_;
	my @existing = Tab::TeamMember->search( student => $student->id, comp => $comp->id );
	Tab::TeamMember->create({ student => $student->id, comp => $comp->id }) unless @existing;
	return;
}

sub rm_member { 
	my ($self, $student) = @_;
	my @existing = Tab::TeamMember->search( student => $student->id, comp => $comp->id );
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
    return Tab::Panel->search_comp_panels($self->id);
}

sub prelims {
    my $self = shift;
    return Tab::Panel->search_comp_prelims($self->id);
}

sub round_panel {
    my ($self,$round) = @_;
    my @panels = Tab::Panel->search_comp_panels_by_round($self->id, $round->id);
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
		comp => $self->id, 
		panel => $panel->id, 
		{order_by => "seed DESC"});
	return $ballots[0]->seed if @ballots;
	return;
} 

sub setorder { 
	my ($self, $order, $panel_id) = @_;
	my $sql_string = "update ballot
		set ballot.speakerorder = $order 
		where ballot.comp = ".$self->id."
		and ballot.panel = $panel_id";
	Tab::Comp->set_sql(set_custom_order => $sql_string);
	Tab::Comp->sql_set_custom_order->execute;
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
			where ballot.comp = ".$self->id."
			and ballot.panel = panel.id
			and panel.round = ".$round->id;
	Tab::Comp->set_sql(order_by_round => $sql_string);
	return Tab::Comp->sql_order_by_round->select_val;
}

sub speakerorder { 
	my ($self,$panel) = @_;
	my @ballots = Tab::Ballot->search( 
		comp => $self->id, 
		panel => $panel->id, 
		{order_by => "speakerorder DESC"});
	return $ballots[0]->speakerorder if @ballots;
	return;
}

sub cume { 

	my $self = shift;
	Tab::Comp->set_sql(overall_cume => "select SUM(ballot.rank) from ballot
						where ballot.comp = ".$self->id);

	return Tab::Comp->sql_overall_cume->select_val;
}

Tab::Comp->set_sql(prelim_cume => "select SUM(ballot.rank) from ballot,panel       
								where ballot.panel = panel.id   
								and panel.type = \"prelim\"       
								and ballot.comp = ?");
sub prelim_cume { 
	my $self = shift;
	return Tab::Comp->sql_prelim_cume->select_val($self->id);
}

sub prelim_ballots { 
	my $self = shift;
	return Tab::Ballot->search_ballots_from_prelims($self->id);
}

sub elim_ballots { 
	my $self = shift;
	return Tab::Ballot->search_ballots_from_elims($self->id);
}

sub final_ballots { 
	my $self = shift;
	return Tab::Ballot->search_ballots_from_finals($self->id);
}



Tab::Comp->set_sql(finals_by_school => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.school = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Comp->set_sql(elims_by_school => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.school = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Comp->set_sql(finals_by_event => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.event = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Comp->set_sql(elims_by_event => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.event = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Comp->set_sql(active_by_event => "select distinct comp.*
									from comp
									where comp.event = ?
									and comp.dropped != 1
									and comp.waitlist != 1");

Tab::Comp->set_sql(waitlist_by_event => "select distinct comp.*
									from comp
									where comp.event = ?
									and comp.dropped != 1
									and comp.waitlist = 1");

Tab::Comp->set_sql(finals_by_tourn => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.tournament = ?
									and ballot.panel = panel.id
									and panel.type = \"final\" ");

Tab::Comp->set_sql(elims_by_tourn => "select distinct comp.id
									from comp,ballot,panel
									where comp.id = ballot.comp
									and comp.tournament = ?
									and ballot.panel = panel.id
									and panel.type = \"elim\" ");

Tab::Comp->set_sql(by_region => "select distinct comp.id 
									from comp,school
									where school.region = ?  
									and school.tournament = ? 
									and school.id = comp.school" );

Tab::Comp->set_sql(double_entries => "select distinct c2.id
        							from comp as c2, comp as c1, event
    							    where c2.student = c1.student
    							    and c2.tournament = c1.tournament
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Comp->set_sql(double_partners => "select distinct c2.id
        							from comp as c2, comp as c1, event
    							    where c2.partner = c1.student
    							    and c2.tournament = c1.tournament
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Comp->set_sql(double_partners_twice => "select distinct c2.id
        							from comp as c2, comp as c1, event
    							    where c1.partner is not null
									and c1.partner != 0
									and c2.partner = c1.partner
    							    and c2.tournament = c1.tournament
   									and c1.event = ?
									and c2.event != c1.event");

Tab::Comp->set_sql(tourn_teams => "select distinct comp.* from comp,team_member
                                    where team_member.student = ? and
                                    team_member.comp = comp.id and
                                    tournament = ?");

Tab::Comp->set_sql(by_tourn_and_code => "select distinct comp.* from comp,tournament,event
									where comp.event = event.id
									and event.tournament = ?
									and comp.code = ?");

Tab::Comp->set_sql(by_timeslot => " select distinct comp.* 
									from comp,ballot,round,panel
		    						where comp.id = ballot.comp
   									and ballot.panel = panel.id
    								and panel.round = round.id
    								and round.timeslot = ? ");

Tab::Comp->set_sql(by_timeslot_and_event => " select distinct comp.* 
									from comp,ballot,round,panel
		    						where comp.id = ballot.comp
   									and ballot.panel = panel.id
    								and panel.round = round.id
    								and round.timeslot = ?
									and panel.event = ? ");

Tab::Comp->set_sql(housed => "select distinct comp.*
									from comp, housing
									where comp.school = ? 
									and comp.tournament = housing.tournament
									and comp.student = housing.student");

Tab::Comp->set_sql(tied => "select distinct c1.id, c2.id 
									from comp as c1, comp as c2,
									ballot as b1, ballot as b2
									where c1.id != c2.id
									and c1.id = b1.comp
									and c2.id = b2.comp
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

Tab::Comp->set_sql(unballoted_comps_by_tourn => "select distinct comp.*
									from comp,event
									where comp.tournament = ? 
									and comp.event = event.id
									and event.type != \"debate\"
									and comp.dropped != 1
									and comp.waitlist != 1
									and not exists (
										select id from ballot where comp = comp.id )
									");


