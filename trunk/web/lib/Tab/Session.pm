package Tab::Session;
use base 'Tab::DBI';
Tab::Session->table('session');
Tab::Session->columns(All => qw/id account authkey userkey timestamp ip limited entry_only circuit tourn school event/);

Tab::Session->has_a(account => 'Tab::Account');
Tab::Session->has_a(circuit => 'Tab::Circuit');
Tab::Session->has_a(school => 'Tab::School');
Tab::Session->has_a(tourn => 'Tab::Tourn');
__PACKAGE__->_register_datetimes( qw/timestamp/);
