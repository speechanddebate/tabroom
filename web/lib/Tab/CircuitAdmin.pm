package Tab::CircuitAdmin;
use base 'Tab::DBI';
Tab::CircuitAdmin->table('circuit_admin');
Tab::CircuitAdmin->columns(All => qw/id circuit timestamp account/);
Tab::CircuitAdmin->has_a(circuit => "Tab::Circuit");
Tab::CircuitAdmin->has_a(account => "Tab::Account");

