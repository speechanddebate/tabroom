package Tab::SchoolFine;
use base 'Tab::DBI';
Tab::SchoolFine->table('school_fine');
Tab::SchoolFine->columns(All => qw/id school levied amount reason tourn start end region timestamp/);
Tab::SchoolFine->has_a(school => 'Tab::School');
Tab::SchoolFine->has_a(region => 'Tab::Region');
Tab::SchoolFine->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/levied/);
