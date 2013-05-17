package Tab::Panel;
use base 'Tab::DBI';
Tab::Panel->table('panel');
Tab::Panel->columns(Primary => qw/id/);
Tab::Panel->columns(Essential => qw/letter round room flight bye/);
Tab::Panel->columns(Others => qw/timestamp bracket confirmed cat_id started score/);
Tab::Panel->columns(TEMP => qw/opp pos side entryid judge audit eventname judgenum panelsize ada/);

Tab::Panel->has_a(room => 'Tab::Room');
Tab::Panel->has_a(round => 'Tab::Round');

Tab::Panel->has_many(ballots => 'Tab::Ballot', 'panel');

__PACKAGE__->_register_datetimes( qw/started timestamp confirmed/);

sub entries {
    my $self = shift;
    return Tab::Entry->search_by_panel($self->id);
}

