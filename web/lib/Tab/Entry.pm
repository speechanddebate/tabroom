package Tab::Entry;
use base 'Tab::DBI';
Tab::Entry->table('entry');
Tab::Entry->columns(Primary => qw/id/);
Tab::Entry->columns(Essential => qw/code dropped name school tourn event/);
Tab::Entry->columns(Others => qw/seed bid title ada waitlist dq drop_time reg_time timestamp/);

Tab::Entry->has_a(school => 'Tab::School');
Tab::Entry->has_a(tourn => 'Tab::Tourn');
Tab::Entry->has_a(event => 'Tab::Event');

Tab::Entry->has_many(ballots => 'Tab::Ballot', 'entry');
Tab::Entry->has_many(ballot_speaks => 'Tab::BallotSpeaks', 'entry');
Tab::Entry->has_many(changes => 'Tab::TournChange', 'entry');
Tab::Entry->has_many(ratings => 'Tab::Rating', 'entry');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/drop_time/);
__PACKAGE__->_register_datetimes( qw/reg_time/);

Tab::Entry->set_sql(by_student => "
				select distinct entry.id
				from entry_student,entry
				where entry_student.student = ?
				and entry_student.entry = entry.id
				and entry.tourn = ?  ");

Tab::Entry->set_sql(by_round=> "
				select distinct entry.*,panel.id as panelid
				from entry,panel,ballot
				where entry.id = ballot.entry
				and entry.dropped != 1
				and entry.dq != 1
				and ballot.panel = panel.id
				and panel.round = ?
				order by ballot.speakerorder ");

Tab::Entry->set_sql(by_panel=> "
			select distinct entry.*,ballot.speakerorder as speaks,
			from entry,ballot,school
			where entry.dropped != 1
			and entry.dq != 1
			and ballot.panel = ? 	
			and ballot.entry = entry.id 
			and entry.school = school.id
			order by ballot.speakerorder,entry.id ");

Tab::Entry->set_sql(highest_code => "select MAX(code) from entry where event = ?");
Tab::Entry->set_sql(lowest_code => "select MIN(code) from entry where event = ?");

sub students {
    my $self = shift;
	return Tab::Student->search_by_entry($self->id));
    return @students;
}

sub how_doubled { 
	my ($self, $panel) = @_;
	return Tab::Panel->search_conflicts( $self->id, $panel->round->timeslot->id);
}

sub add_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student->id, entry => $entry->id );
	Tab::EntryStudent->create({ student => $student->id, entry => $entry->id }) unless @existing;
	return;
}

sub rm_student { 
	my ($self, $student) = @_;
	my @existing = Tab::EntryStudent->search( student => $student->id, entry => $entry->id );
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

