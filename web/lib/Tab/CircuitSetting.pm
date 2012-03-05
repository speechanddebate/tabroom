package Tab::CircuitSetting;
use base 'Tab::DBI';
Tab::CircuitSetting->table('circuit_setting');
Tab::CircuitSetting->columns(All => qw/id circuit tag value text modified timestamp/);
#Tab::CircuitSetting->has_a(circuit => 'Tab::Circuit');
Tab::CircuitSetting->has_a(modified => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);

