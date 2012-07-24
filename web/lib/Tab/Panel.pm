package Tab::Panel;
use base 'Tab::DBI';
Tab::Panel->table('panel');
Tab::Panel->columns(Primary => qw/id/);
Tab::Panel->columns(Essential => qw/letter round room flight bye timestamp/);
Tab::Panel->columns(TEMP => qw/judge/);

Tab::Panel->has_a(room => 'Tab::Room');
Tab::Panel->has_a(round => 'Tab::Round');

Tab::Panel->has_many(ballots => 'Tab::Ballot', 'panel');

__PACKAGE__->_register_dates( qw/timestamp/);

sub entries {
    my $self = shift;
    return Tab::Entry->search_by_panel($self->id);
}

