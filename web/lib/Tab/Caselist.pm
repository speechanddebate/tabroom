package Tab::Caselist;
use base 'Tab::DBI';
Tab::Caselist->table('caselist');
Tab::Caselist->columns(All => qw/id slug eventcode person partner timestamp/);
Tab::Caselist->has_a(person => 'Tab::Person');
Tab::Caselist->has_a(partner => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp/);
