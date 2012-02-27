package Tab::Sweep;
use base 'Tab::DBI';
Tab::Sweep->table('sweep');
Tab::Sweep->columns(All => qw/id tournament timestamp place value prelim_cume/);
Tab::Sweep->has_a(tournament => 'Tab::Tournament');
__PACKAGE__->_register_datetimes( qw/timestamp/);
