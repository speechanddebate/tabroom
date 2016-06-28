package Tab::SchoolFine;
use base 'Tab::DBI';
Tab::SchoolFine->table('school_fine');
Tab::SchoolFine->columns(All => qw/id reason amount payment
								deleted deleted_at deleted_by levied_at levied_by 
								tourn school region timestamp  /);

Tab::SchoolFine->has_a(tourn => 'Tab::Tourn');
Tab::SchoolFine->has_a(school => 'Tab::School');
Tab::SchoolFine->has_a(region => 'Tab::Region');
Tab::SchoolFine->has_a(levied_by => 'Tab::Person');
Tab::SchoolFine->has_a(deleted_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/levied_at deleted_at timestamp/);
