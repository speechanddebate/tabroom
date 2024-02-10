package Tab::ChangeLog;
use base 'Tab::DBI';
Tab::ChangeLog->table('change_log');
Tab::ChangeLog->columns(Primary => qw/id/);
Tab::ChangeLog->columns(Essential => qw/tag tourn school person description/);
Tab::ChangeLog->columns(Others => qw/judge entry event category chapter circuit round panel new_panel old_panel fine deleted created_at timestamp/);

Tab::ChangeLog->has_a(person   => "Tab::Person");
Tab::ChangeLog->has_a(tourn    => "Tab::Tourn");
Tab::ChangeLog->has_a(event    => "Tab::Event");
Tab::ChangeLog->has_a(category => "Tab::Category");
Tab::ChangeLog->has_a(chapter  => "Tab::Chapter");
Tab::ChangeLog->has_a(circuit  => "Tab::Circuit");
Tab::ChangeLog->has_a(entry    => "Tab::Entry");
Tab::ChangeLog->has_a(school   => "Tab::School");
Tab::ChangeLog->has_a(judge    => "Tab::Judge");
Tab::ChangeLog->has_a(fine     => "Tab::Fine");

Tab::ChangeLog->has_a(round     => "Tab::Round");
Tab::ChangeLog->has_a(panel     => "Tab::Panel");
Tab::ChangeLog->has_a(new_panel => "Tab::Panel");
Tab::ChangeLog->has_a(old_panel => "Tab::Panel");

__PACKAGE__->_register_datetimes( qw/timestamp created_at/);

sub account {
	my $self = shift;
	return $self->person;
}
