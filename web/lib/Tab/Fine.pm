package Tab::Fine;
use base 'Tab::DBI';
Tab::Fine->table('fine');
Tab::Fine->columns(Essential => qw/id tourn amount reason levied_at levied_by timestamp/);
Tab::Fine->columns(Others => qw/school region judge/);

Tab::Fine->has_a(school => 'Tab::School');
Tab::Fine->has_a(region => 'Tab::Region');
Tab::Fine->has_a(judge => 'Tab::Judge');
Tab::Fine->has_a(tourn => 'Tab::Tourn');
Tab::Fine->has_a(levied_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/levied_at/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
