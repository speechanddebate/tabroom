package Tab::Strike;
use base 'Tab::DBI';
Tab::Strike->table('strike');
Tab::Strike->columns(Primary => qw/id/); 
Tab::Strike->columns(Essential => qw/type start end registrant conflictee 
							tourn judge event entry school district region timeslot strike_timeslot 
							entered_by timestamp/);

Tab::Strike->has_a(tourn => 'Tab::Tourn');
Tab::Strike->has_a(judge => 'Tab::Judge');
Tab::Strike->has_a(event => 'Tab::Event');
Tab::Strike->has_a(entry => 'Tab::Entry');

Tab::Strike->has_a(school => 'Tab::School');
Tab::Strike->has_a(region => 'Tab::Region');
Tab::Strike->has_a(district => 'Tab::District');
Tab::Strike->has_a(timeslot => 'Tab::Timeslot');
Tab::Strike->has_a(strike_timeslot => 'Tab::StrikeTimeslot');
Tab::Strike->has_a(entered_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/start end timestamp/);

sub name { 

	# Wow this really shouldn't live HERE of all places -- CLP 6.16.17

	my $self = shift;
	my $tz = $self->tourn->tz if $self->tourn;
	$tz = "UTC" unless $tz;
	return "No prelims in ".$self->event->name if $self->type eq "elim";

	my $type = $self->type;

	my $frame = '<span class="threetenths semibold">';

	if ($self->registrant) { 

		if ($type eq "event") { 

			return $frame . "No Rounds</span> "
				.$self->event->name;

		} elsif ($type eq "school") { 

			return $frame . "Strike</span> "
				.$self->school->short_name;

		} elsif ($type eq "entry") { 

			return $frame . "Strike</span> "
				.$self->entry->event->abbr." "
				.$self->entry->code." "
				.$self->entry->name;

		} elsif ($type eq "region") {

			return $frame . "Strike</span> "
				.$self->region->name." (".$self->region->code.")";

		} elsif ($self->entry && $type eq "conflict") { 

			return $frame . "Conflict</span> "
				.$self->entry->event->abbr." "
				.$self->entry->code." "
				.$self->entry->school->short_name ;

		} elsif ($self->school && $type eq "conflict") { 

			return $frame . "Conflict</span> "
				.$self->school->short_name;
		}

	} else { 

		if ($type eq "event") {

			return $frame . "Tab Strike</span> "
				.$self->event->name;

		} elsif ($type eq "school") {

			return $frame . "Tab Strike</span> "
				.$self->school->short_name;

		} elsif ($type eq "entry") {

			return $frame . "Tab Strike</span> "
				.$self->entry->event->abbr
				." ".$self->entry->code
				." ".$self->entry->school->short_name;

		} elsif ($type eq "region") {

			return $frame . "Tab Strike</span> "
				.$self->region->name
				." (".$self->region->code.")";

		} elsif ($type eq "district") { 

			return $frame . "Tab Strike</span> "
				.$self->district->name
				." (".$self->district->code.")";
		}
	}


	if ($type eq "time" && $self->start->day != $self->end->day) { 
		return $frame." Timeblock</span>"
			. Tab::niceshortdayt($self->start->set_time_zone($tz))
			." until ".  Tab::niceshortdayt($self->end->set_time_zone($tz));
	};

	if ($self->type eq "time" && $self->start->day == $self->end->day) { 
		return $frame." Timeblock</span>"
			. Tab::niceshortdayt($self->start->set_time_zone($tz))
			.Tab::niceshortdayt($self->start->set_time_zone($tz))
			." until ".  Tab::nicetime($self->end->set_time_zone($tz));
	}
}


