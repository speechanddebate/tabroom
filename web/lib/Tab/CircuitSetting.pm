package Tab::CircuitSetting;
use base 'Tab::DBI';
Tab::CircuitSetting->table('circuit_setting');
Tab::CircuitSetting->columns(All => qw/id circuit tag value text timestamp/);
Tab::CircuitSetting->has_a(circuit => 'Tab::Circuit');

__PACKAGE__->_register_datetimes( qw/timestamp/);

