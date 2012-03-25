package Tab::CircuitSetting;
use base 'Tab::DBI';
Tab::CircuitSetting->table('circuit_setting');
Tab::CircuitSetting->columns(All => qw/id circuit tag value value_date value_text timestamp/);
Tab::CircuitSetting->has_a(circuit => 'Tab::Circuit');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/value_date/);

