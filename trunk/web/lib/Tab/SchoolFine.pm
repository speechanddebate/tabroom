package Tab::SchoolFine;
use base 'Tab::DBI';
Tab::SchoolFine->table('school_fine');
Tab::SchoolFine->columns(All => qw/id school tourn amount reason levied_on levied_by timestamp region judge payment deleted deleted_by deleted_at/);

Tab::SchoolFine->has_a(school => 'Tab::School');
Tab::SchoolFine->has_a(region => 'Tab::Region');
Tab::SchoolFine->has_a(judge => 'Tab::Judge');
Tab::SchoolFine->has_a(tourn => 'Tab::Tourn');
Tab::SchoolFine->has_a(levied_by => 'Tab::Account');
Tab::SchoolFine->has_a(deleted_by => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/levied_on deleted_at timestamp/);
