package Tab::Strike;
use base 'Tab::DBI';
Tab::Strike->table('strike');
Tab::Strike->columns(Primary => qw/id/);
Tab::Strike->columns(Essential => qw/tourn judge type event entry school region
                                     start end strike_time registrant conflictee timestamp/);

Tab::Strike->has_a(judge => 'Tab::Judge');
Tab::Strike->has_a(tourn => 'Tab::Tourn');
Tab::Strike->has_a(event => 'Tab::Event');
Tab::Strike->has_a(entry => 'Tab::Entry');
Tab::Strike->has_a(school => 'Tab::School');
Tab::Strike->has_a(region => 'Tab::Region');
Tab::Strike->has_a(strike_time => 'Tab::StrikeTime');

__PACKAGE__->_register_datetimes( qw/start/ );
__PACKAGE__->_register_datetimes( qw/end/);

sub name { 
	my $self = shift;
	my $tz = $self->tourn->tz if $self->tourn;
	$tz = "UTC" unless $tz;
	return "No rounds in ".$self->event->name if $self->type eq "event";
	return "No prelims in ".$self->event->name if $self->type eq "elim";
	return "No one from ".$self->school->short_name if $self->type eq "school";
	return "No one from ".$self->region->name." (".$self->region->code.")" if $self->type eq "region";
	return $self->entry->event->abbr." ".$self->entry->school->short_name." ".$self->entry->code if $self->type eq "entry";
	return $self->entry->event->abbr." ".$self->entry->school->short_name." ".$self->entry->code if $self->entry && $self->type eq "conflict";
	return $self->school->short_name if $self->school && $self->type eq "conflict";

	return "Out ". Tab::niceshortdayt($self->start->set_time_zone($tz))
			." to ".  Tab::niceshortdayt($self->end->set_time_zone($tz)) if $self->type eq "time" && $self->start->day != $self->end->day;

	return "Out ".  Tab::niceshortdayt($self->start->set_time_zone($tz))
			." to ".  Tab::nicetime($self->end->set_time_zone($tz)) if $self->type eq "time" && $self->start->day == $self->end->day;
}


