package Tab::Student;
use base 'Tab::DBI';
Tab::Student->table('student');
Tab::Student->columns(Primary => qw/id/);
Tab::Student->columns(Essential => qw/account first last chapter novice grad_year retired gender acct_request/);
Tab::Student->columns(Other => qw/timestamp phonetic created/);
Tab::Student->columns(TEMP => qw/code entry/);

Tab::Student->has_a(chapter => 'Tab::Chapter');
Tab::Student->has_a(account => 'Tab::Account');
Tab::Student->has_a(acct_request => 'Tab::Account');


Tab::Student->has_many(entry_students => 'Tab::EntryStudent', 'student');

__PACKAGE__->_register_datetimes( qw/timestamp created/);

sub entries { 
    my ($self,$tourn) = @_;
    return Tab::Entry->search_by_student_and_tourn($self->id, $tourn->id) if $tourn;
    return Tab::Entry->search_by_student($self->id);
}

sub housing { 
	my ($self, $tourn, $day) = @_;
	my @housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id, night => $day->ymd ) if $day;
	@housings = Tab::Housing->search( student => $self->id, tourn => $tourn->id ) unless $day;
	return shift @housings if $day;
	return @housings;
}

