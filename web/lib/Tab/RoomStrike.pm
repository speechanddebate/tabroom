package Tab::RoomStrike;
use base 'Tab::DBI';
Tab::RoomStrike->table('room_strike');
Tab::RoomStrike->columns(All => qw/id room type event judge tourn entry start end timestamp/);

Tab::RoomStrike->has_a(room => 'Tab::Room');

Tab::RoomStrike->has_a(event => 'Tab::Event');
Tab::RoomStrike->has_a(entry => 'Tab::Entry');
Tab::RoomStrike->has_a(tourn => 'Tab::Tourn');
Tab::RoomStrike->has_a(judge => 'Tab::Judge');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/start end/);

sub name {
	my $self = shift;
	my $tz = $self->tourn->tz if $self->tourn;
	$tz = "UTC" unless $tz;

	if ($self->type eq "event" && $self->event > 0) {
		return "No rounds of ".$self->event->name;
	};

	return Tab::niceshortdayt($self->start->set_time_zone($tz)) ." until ".  Tab::niceshortdayt($self->end->set_time_zone($tz))
		if $self->type eq "time" && $self->start->day != $self->end->day;

	return Tab::niceshortdayt($self->start->set_time_zone($tz)) ." until ".  Tab::nicetime($self->end->set_time_zone($tz))
		if $self->type eq "time" && $self->start->day == $self->end->day;

	return "No judge ".$self->judge->code." ".$self->judge->first." ".$self->judge->last if $self->type eq "judge";
	return "No entry ".$self->entry->code." ".$self->entry->name if $self->type eq "entry";
}


