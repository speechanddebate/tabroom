package Tab::Fine;
use base 'Tab::DBI';
Tab::Fine->table('fine');
Tab::Fine->columns(All => qw/id school levied amount reason tournament start end region timestamp/);
Tab::Fine->has_a(school => 'Tab::School');
Tab::Fine->has_a(region => 'Tab::Region');
Tab::Fine->has_a(tournament => 'Tab::Tournament');

__PACKAGE__->_register_datetimes( qw/start/);
__PACKAGE__->_register_datetimes( qw/end/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/levied/);
