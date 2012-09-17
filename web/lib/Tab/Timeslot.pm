package Tab::Timeslot;
use base 'Tab::DBI';
Tab::Timeslot->table('timeslot');
Tab::Timeslot->columns(Primary => qw/id/);
Tab::Timeslot->columns(Essential => qw/tourn name start end name timestamp/);
Tab::Timeslot->columns(TEMP => qw/judgeid/);

Tab::Timeslot->has_a(tourn => 'Tab::Tourn');
Tab::Timeslot->has_many(rounds => 'Tab::Round', 'timeslot');

__PACKAGE__->_register_datetimes( qw/start end timestamp/);

sub done { 
	my $self = shift;
	my $undone = Tab::Ballot->search_undone_by_timeslot($self->id);
	return if $undone;
	return 1;
}

sub span { 
	my $self = shift; 
	return DateTime::Span->from_datetimes( after => $self->start, before => $self->end ); 
}

sub entries { 
	my $self = shift;
	return Tab::Entry->search_by_timeslot($self->id);
}
