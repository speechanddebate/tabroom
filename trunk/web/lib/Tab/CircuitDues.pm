package Tab::CircuitDues;
use base 'Tab::DBI';
Tab::CircuitDues->table('circuit_dues');
Tab::CircuitDues->columns(All => qw/id chapter amount paid_on circuit timestamp/);
Tab::CircuitDues->has_a(chapter => 'Tab::Chapter');
Tab::CircuitDues->has_a(circuit => 'Tab::Circuit');
__PACKAGE__->_register_datetimes( qw/paid_on/);
__PACKAGE__->_register_datetimes( qw/timestamp/);
