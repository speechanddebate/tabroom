package Tab::Fine;
use base 'Tab::DBI';
Tab::Fine->table('fine');
Tab::Fine->columns(All => qw/id reason amount payment
								deleted deleted_at deleted_by levied_at levied_by 
								tourn school region judge timestamp/);

Tab::Fine->has_a(judge => 'Tab::Judge');
Tab::Fine->has_a(tourn => 'Tab::Tourn');
Tab::Fine->has_a(school => 'Tab::School');
Tab::Fine->has_a(region => 'Tab::Region');
Tab::Fine->has_a(levied_by => 'Tab::Person');
Tab::Fine->has_a(deleted_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/levied_at deleted_at timestamp/);
