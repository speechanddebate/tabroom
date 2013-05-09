package Tab::Entry;
use base 'Tab::DBI';
Tab::Entry->table('entry');
Tab::Entry->columns(Primary => qw/id/);
Tab::Entry->columns(Essential => qw/code dropped name school tourn event seed bid title ada waitlist 
									dq drop_time reg_time timestamp drop_by reg_by off_waitlist tba 
									self_reg_by unconfirmed sweeps placement pair_seed/);
Tab::Entry->columns(TEMP => qw/panelid speaks side ballot othername schname regname regcode region/);

Tab::Entry->has_a(school => 'Tab::School');
Tab::Entry->has_a(tourn => 'Tab::Tourn');
Tab::Entry->has_a(event => 'Tab::Event');

Tab::Entry->has_a(reg_by => 'Tab::Account');
Tab::Entry->has_a(drop_by => 'Tab::Account');

Tab::Entry->has_many(strikes => 'Tab::Strike', 'entry');
Tab::Entry->has_many(ballots => 'Tab::Ballot', 'entry');
Tab::Entry->has_many(changes => 'Tab::TournChange', 'entry');
Tab::Entry->has_many(ratings => 'Tab::Rating', 'entry');
Tab::Entry->has_many(qualifiers => 'Tab::Qualifier', 'entry');
Tab::Entry->has_many(entry_students => 'Tab::EntryStudent', 'entry');
Tab::Entry->has_many(students => [Tab::EntryStudent => 'student']);

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

