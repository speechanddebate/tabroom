package Tab::Entry;
use base 'Tab::DBI';
Tab::Entry->table('entry');
Tab::Entry->columns(Primary => qw/id/);
Tab::Entry->columns(Essential => qw/code dropped name school tourn event seed bid title ada waitlist dq drop_time reg_time timestamp/);

Tab::Entry->has_a(school => 'Tab::School');
Tab::Entry->has_a(tourn => 'Tab::Tourn');
Tab::Entry->has_a(event => 'Tab::Event');

Tab::Entry->has_many(ballots => 'Tab::Ballot', 'entry');
Tab::Entry->has_many(ballot_speaks => 'Tab::BallotValue', 'entry');
Tab::Entry->has_many(changes => 'Tab::TournChange', 'entry');
Tab::Entry->has_many(ratings => 'Tab::Rating', 'entry');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/drop_time/);
__PACKAGE__->_register_datetimes( qw/reg_time/);

sub add_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student, entry => $self->id );
	Tab::EntryStudent->create({ student => $student, entry => $self->id }) unless @existing;
	return;
}

sub rm_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student, entry => $self->id );
	foreach (@existing) { $_->delete; }
	return;
}

sub team_name {

	my $self = shift;
	return $self->name if $self->name;
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
		$not_first++;
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
		$not_first++;
	}

	return $name;

}

