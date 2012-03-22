package Tab::Tournament;
use base 'Tab::DBI';
Tab::Tournament->table('tournament');
Tab::Tournament->columns(Primary => qw/id/);
Tab::Tournament->columns(Essential => qw/name start end league director approved method/);

Tab::Tournament->columns(Others => qw/timestamp hidden reg_start reg_end webname
							judge_deadline drop_deadline freeze_deadline
							fine_deadline judge_policy invitename inviteurl
							results bill_packet disclaimer invoice_message
							ballot_message chair_ballot_message web_message
							vlabel hlabel vcorner hcorner/);

Tab::Tournament->has_a(director => 'Tab::Account');
Tab::Tournament->has_a(league => 'Tab::League');
Tab::Tournament->has_a(method => 'Tab::Method');
Tab::Tournament->has_many(events => 'Tab::Event', 'tournament');
Tab::Tournament->has_many(items => 'Tab::Item', 'tournament');
Tab::Tournament->has_many(news => 'Tab::News', 'tournament');
Tab::Tournament->has_many(emails => 'Tab::Email', 'tournament');
Tab::Tournament->has_many(resultfiles => 'Tab::ResultFile', 'tournament');
Tab::Tournament->has_many(comps => 'Tab::Comp', 'tournament');
Tab::Tournament->has_many(strikes => 'Tab::Strike', 'tournament');
Tab::Tournament->has_many(judges => 'Tab::Judge', 'tournament');
Tab::Tournament->has_many(timeslots => 'Tab::Timeslot', 'tournament');
Tab::Tournament->has_many(changes => 'Tab::Change', 'tournament');
Tab::Tournament->has_many(accounts => 'Tab::AccountAccess', 'tournament');
Tab::Tournament->has_many(classes => 'Tab::Class', 'tournament');
Tab::Tournament->has_many(schools => 'Tab::School', 'tournament');
Tab::Tournament->has_many(tiebreak_sets => 'Tab::TiebreakSet', 'tournament');
Tab::Tournament->has_many(files => 'Tab::File', 'tournament');
Tab::Tournament->has_many(ratings => 'Tab::Rating', 'tournament');
Tab::Tournament->has_many(groups => 'Tab::JudgeGroup', 'tournament');
Tab::Tournament->has_many(quals => 'Tab::Qual', 'tournament');
Tab::Tournament->has_many(roomblocks => 'Tab::RoomBlock', 'tournament');
Tab::Tournament->has_many(bills => 'Tab::Bill', 'tournament');
Tab::Tournament->has_many(fines => 'Tab::Fine', 'tournament');
Tab::Tournament->has_many(sweeps => 'Tab::Sweep', 'tournament');
Tab::Tournament->has_many(housing_requests => 'Tab::Housing', 'tournament');
__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);
__PACKAGE__->_register_datetimes( qw/reg_start/);
__PACKAGE__->_register_datetimes( qw/reg_end/);
__PACKAGE__->_register_datetimes( qw/freeze_deadline/);
__PACKAGE__->_register_datetimes( qw/drop_deadline/);
__PACKAGE__->_register_datetimes( qw/judge_deadline/);
__PACKAGE__->_register_datetimes( qw/fine_deadline/);

Tab::Tournament->set_sql(my_tourns => "
		select distinct tournament.id 
		from tournament,account_access 
		where tournament.id = account_access.tournament 
		and account_access.account = ?");

Tab::Tournament->set_sql(by_site => "
		select distinct tournament.id 
		from tournament,tournament_site 
		where tournament.id = tournament_site.tournament 
		and tournament_site.site = ?");

Tab::Tournament->set_sql(open_by_chapter => "
		select distinct tournament.*
		from tournament,league,chapter_league
		where chapter_league.chapter = ?
		and tournament.approved = 1
		and tournament.hidden != 1
		and chapter_league.active = 1
		and chapter_league.league = league.id
		and league.active = 1
		and league.approved = 1
		and league.diocese_based != 1
		and league.id = tournament.league
		and tournament.reg_start < NOW()
		and tournament.reg_end > NOW()
			and not exists (
				select id from school where 
				school.chapter = chapter_league.chapter
				and school.tournament = tournament.id
			)
		order by tournament.reg_end DESC;
	");

Tab::Tournament->set_sql(entered_by_chapter => "
		select distinct tournament.*
		from tournament,school
		where school.chapter = ?
		and school.tournament = tournament.id
		and tournament.end > NOW()
		order by tournament.start DESC;
	");

Tab::Tournament->set_sql(results_by_chapter => "
		select distinct tournament.*
		from tournament,school
		where school.chapter = ?
		and school.tournament = tournament.id
		and tournament.results = 1
		order by tournament.end DESC
	");

sub pools { 
	my $self = shift;
	return Tab::Pool->search_by_tournament($self->id);
}

sub room_pools { 
	my $self = shift; 
	return Tab::RoomPool->search_by_tourn($self->id);
}

sub chapters { 
	my $self = shift;
	return Tab::Chapter->search_by_tourn($self->id);
}

sub begins {
	my $self = shift;
	my $ts = $self->start;
	$ts->set_time_zone($self->league->timezone);
	return $ts;
}

sub ends {
	my $self = shift;
	my $ts = $self->end;
	$ts->set_time_zone($self->league->timezone);
	return $ts;
}

sub disqualified {
	my $self = shift;
	return Tab::Comp->search_disqualified($self->id);
}

sub sites { 
	my $self = shift; 
	my @sites = Tab::Site->search_by_tournament($self->id);
	return @sites;
}

sub rooms {
	my $self = shift;
	return Tab::Room->search_tourn_rooms($self->id);
}

sub tabbers {
    my $self = shift;
    my @tabbers;
    push (@tabbers, $self->director);
    push (@tabbers, Tab::Account->search_tabbers($self->id));
	return @tabbers;
}


sub finalists { 
	my $self = shift;
	return Tab::Comp->search_finals_by_tourn($self->id);
}


sub elims { 
	my $self = shift;
	return Tab::Comp->search_elims_by_tourn($self->id);
}

sub elim_panels { 
	my $self = shift;
	return Tab::Panel->search_elims_by_tourn($self->id);
}

sub panels_with_judgecount { 
	my $self = shift;
	return Tab::Panel->search_judgecount($self->id);
}

sub panels_without_judges { 
	my $self = shift;
	return Tab::Panel->search_by_nojudge($self->id);
}

sub panels_without_rooms { 
	my $self = shift;
	return Tab::Panel->search_by_noroom($self->id);
}

sub panels { 
	my $self = shift;
	return Tab::Panel->search_by_tourn($self->id);
}

sub double_booked_judges { 
	my $self = shift;
	return Tab::Judge->search_double_booked($self->id);
}

sub ballots { 
	my $self = shift;
	return Tab::Ballot->search_by_tournament($self->id);
}

sub regions {
	my $self = shift;
	return Tab::Region->search_by_tournament($self->id);
}

sub rounds { 
	my $self = shift;
	return Tab::Round->search_by_tournament($self->id);
}

sub published_rounds { 
	my $self = shift;
	return Tab::Round->search_published_by_tournament($self->id);
}

sub listed_rounds { 
	my $self = shift;
	return Tab::Round->search_listed_by_tournament($self->id);
}

sub field_reports { 
	my $self = shift;
	return Tab::Event->search_reported_by_tournament($self->id);
}

sub live_updates { 
	my $self = shift;
	return Tab::Event->search_liveupdates_by_tournament($self->id);
}

sub prelims { 
	my $self = shift;
	return Tab::Round->search_prelims_by_tournament($self->id);
}


sub min_round { 
	my $self = shift; 
		
	Tab::Tournament->set_sql( min_round_name => "select min(round.name)
							from round,event
							where round.event = event.id
							and event.tournament = ".$self->id);

	return	Tab::Tournament->sql_min_round_name->select_val;
}

sub max_round { 
	my $self = shift; 
		
	Tab::Tournament->set_sql( max_round_name => "select max(round.name)
							from round,event
							where round.event = event.id
							and event.tournament = ".$self->id);

	return	Tab::Tournament->sql_max_round_name->select_val;
}
	

sub max_panel_letter { 
	my $self = shift; 
		
	Tab::Tournament->set_sql( max_panel_letter => "select max(panel.letter)
							from panel,event 
							where panel.event = event.id
							and event.tournament = ".$self->id);

	return	Tab::Tournament->sql_max_panel_letter->select_val;
}
	
sub min_panel_letter { 
	my $self = shift; 
		
	Tab::Tournament->set_sql( min_panel_letter => "select min(panel.letter)
							from panel,event 
							where panel.event = event.id
							and event.tournament = ".$self->id);

	return	Tab::Tournament->sql_min_panel_letter->select_val;
}

sub contacts { 
	my $self = shift;
	return Tab::Account->search_by_tournament( $self->id);
}

sub students { 
	my $self = shift;
	my @students =  Tab::Student->search_by_tournament( $self->id );
	return @students;
}

sub comp_with_code {
	my ($self, $code) = @_;	
	my @comps = Tab::Comp->search_by_tourn_and_code( $self->id, $code);

	my $comp = shift @comps if @comps;
	return $comp;
}

sub days { 
	
	my $self = shift;
    my $league  = $self->league;
    my $start   = $self->start;
    my $end     = $self->end;

    $start->set_time_zone($league->timezone);
    $end->set_time_zone($league->timezone);

    $end->truncate(to   => 'day');
    $start->truncate(to => 'day');

    my @self_days;
    unless ($start == $end) {
        my $holder = $start->clone;
        until ($holder > $end) {
            push (@self_days, $holder->clone);
            $holder->add( days => 1);
        }
    } else { 

		push (@self_days, $start);

	}

	return @self_days;
}

sub unassigned_comps { 
	my $self = shift;
	return Tab::Comp->search_unballoted_comps_by_tourn( $self->id );
}

sub speech_schools { 
	my $self = shift;
	return Tab::School->search_speech_schools_by_tourn($self->id);
}

sub speech_comps { 
	my $self = shift;
	return Tab::Comp->search_speech_comps_by_tourn($self->id);
}

sub speech_ballots { 
	my $self = shift;
	return Tab::Ballot->search_speech_ballots_by_tourn($self->id);
}

sub debate_and_congress_schools { 
	my $self = shift;
	return Tab::School->search_debate_congress_schools_by_tourn($self->id);
}

sub debate_and_congress_comps { 
	my $self = shift;
	return Tab::Comp->search_debate_congress_comps_by_tourn($self->id);
}

sub timeslots_with_panels { 
	my $self = shift;
	return Tab::Timeslot->search_with_panels_by_tourn($self->id);
}

sub setting {

	my ($self, $tag, $value, $blob) = @_;

	my @existing = Tab::TournSetting->search(  
		tourn => $self->id,
		tag => $tag
	);

	if ($value &! $value == 0) { 

		if (@existing) {

			my $exists = shift @existing;
			$exists->value($value);
			$exists->value_text($blob) if $value eq "text";
			$exists->value_date($blob) if $value eq "date";
			$exists->update;

			foreach my $other (@existing) { 
				$other->delete;
			}

			return;

		} else {

			my $setting = Tab::TournSetting->create({
				tourn => $self->id,
				tag => $tag,
				value => $value,
			});

			$setting->value_text($blob) if $value eq "text";
			$setting->value_date($blob) if $value eq "date";
			$setting->update;

		}

	} else {

		return unless @existing;

		my $setting = shift @existing;

		foreach my $other (@existing) { 
			$other->delete;
		}

		return $setting->text if $setting->value eq "text";
		return $setting->datetime if $setting->value eq "date";
		return $setting->value;

	}
}
