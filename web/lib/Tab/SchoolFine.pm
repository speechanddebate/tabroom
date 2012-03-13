package Tab::SchoolFine;
use base 'Tab::DBI';
Tab::SchoolFine->table('school_fine');
Tab::SchoolFine->columns(All => qw/id school tourn amount reason levied_on levied_by timestamp/);

Tab::SchoolFine->has_a(school => 'Tab::School');
Tab::SchoolFine->has_a(levied_by => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/levied_on/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
