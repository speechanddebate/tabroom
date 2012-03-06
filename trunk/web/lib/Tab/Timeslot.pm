package Tab::Timeslot;
use base 'Tab::DBI';
Tab::Timeslot->table('timeslot');
Tab::Timeslot->columns(Primary => qw/id/);
Tab::Timeslot->columns(Essential => qw/start timestamp end name tourn/);
Tab::Timeslot->has_a(tourn => 'Tab::Tourn');
Tab::Timeslot->has_many(rounds => 'Tab::Round', 'timeslot');
Tab::Timeslot->has_many(standby_pools => 'Tab::Pool', 'timeslot');

__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);

__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);


Tab::Timeslot->set_sql(with_panels_by_tourn => "
			select distinct timeslot.* from
			timeslot,round
			where timeslot.tourn = ?
			and timeslot.id = round.timeslot
			and exists (
				select panel.id 
				from panel 
				where panel.round = round.id)");

Tab::Timeslot->set_sql(with_panels_by_site_and_tourn => "
			select distinct timeslot.* from
			timeslot,round
			where round.site = ? 
			and round.timeslot = timeslot.id
			and timeslot.tourn = ?
			and exists (
				select panel.id 
				from panel 
				where panel.round = round.id)");

Tab::Timeslot->set_sql(by_pool => "select distinct timeslot.* from
			timeslot,round,pool_round
			where pool_round.pool = ? 
			and pool_round.round = round.id
			and round.timeslot = timeslot.id");

Tab::Timeslot->set_sql(by_prelim_pool => "select distinct timeslot.* from
			timeslot,round
			where round.pool = ? 
			and round.timeslot = timeslot.id");

Tab::Timeslot->set_sql(by_prelim_group => "
			select distinct timeslot.* from
			timeslot,round,event,class
			where round.event = event.id
			and round.type = \"prelim\"
			and event.class = class.id
			and class.judge_group = ? 
			and round.timeslot = timeslot.id");


Tab::Timeslot->set_sql(prelims => " 
			select timeslot.* 
			from timeslot,round,panel
			where timeslot.tourn = ? 
			and panel.type = \"prelim\"
			and panel.round = round.id
			and round.timeslot = timeslot.id");

sub begins {
	my $self = shift;
	my $ts = $self->start;
	$ts->set_time_zone($self->tourn->league->timezone);
	return $ts;
}

sub ends {
	my $self = shift;
	my $ts = $self->end;
	$ts->set_time_zone($self->tourn->league->timezone);
	return $ts;
}

sub done { 
	my $self = shift;
	my $undone = Tab::Ballot->search_undone_by_timeslot($self->id);
	return if $undone;
	return 1;
}

sub checked { 
	my $self = shift;
	return Tab::Ballot->sql_count_unchecked->select_val($self->id);
}

sub presets { 
	my $self = shift;
	return Tab::Panel->search_preset_by_timeslot($self->id);
}

sub span { 
	my $self = shift; 
	return DateTime::Span->from_datetimes( after => $self->start, before => $self->end ); 
}

sub rooms { 

	my ($self, $site_id) = @_;
	return unless $site_id;
	return Tab::Room->search_clean_rooms_by_timeslot($self->id, $site_id);
}

sub judges { 
	my ($self, $site_id) = @_;

	if ($site_id) { 

		return Tab::Judge->search_by_timeslot_and_site($self->id, $site_id);

	} else {

		return Tab::Judge->search_by_timeslot($self->id);
		
	}
}

sub panels {

    my ($self, $event_id) = @_;
	my @panels;

	if ($event_id) { 

		@panels = Tab::Panel->search_by_event_and_timeslot($event_id, $self->id);

	} else { 

   		@panels = Tab::Panel->search_by_timeslot($self->id);

	}

	return @panels;
}

sub ballots {

    my ($self, $event_id) = @_;
	my @ballots;

	if ($event_id) { 

		@ballots = Tab::Ballot->search_by_event_and_timeslot($event_id, $self->id);

	} else { 

   		@ballots = Tab::Ballot->search_by_timeslot($self->id);

	}

	return @ballots;
}

sub roomless_panels {

    my ($self, $event_id) = @_;
	my @panels;

	if ($event_id) { 

		@panels = Tab::Panel->search_roomless_by_event_and_timeslot($event_id, $self->id);

	} else { 
   		@panels = Tab::Panel->search_roomless_by_timeslot($self->id);
	}

	return @panels;
}

sub entrys { 
	my $self = shift;
	return Tab::Entry->search_by_timeslot($self->id);
}

sub entrys_by_event { 
	my ($self, $event) = @_;
	return Tab::Entry->search_by_timeslot_and_event($self->id, $event->id);
}


