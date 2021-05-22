package Tab::CampusLog;
use base 'Tab::DBI';
Tab::CampusLog->table('campus_log');
Tab::CampusLog->columns(Primary => qw/id/);
Tab::CampusLog->columns(Essential => qw/tag uuid description tourn school person panel entry judge student timestamp/);

Tab::CampusLog->has_a(person  => "Tab::Person");
Tab::CampusLog->has_a(panel   => "Tab::Panel");
Tab::CampusLog->has_a(tourn   => "Tab::Tourn");
Tab::CampusLog->has_a(school  => "Tab::School");
Tab::CampusLog->has_a(student => "Tab::Student");
Tab::CampusLog->has_a(entry   => "Tab::Entry");
Tab::CampusLog->has_a(judge   => "Tab::Judge");

__PACKAGE__->_register_datetimes( qw/timestamp/);

