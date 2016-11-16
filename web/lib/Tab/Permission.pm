package Tab::Permission;
use base 'Tab::DBI';
Tab::Permission->table('permission');
Tab::Permission->columns(All   => qw/id tag person category tourn chapter region district circuit timestamp/);

Tab::Permission->has_a(circuit => "Tab::Circuit");
Tab::Permission->has_a(district => "Tab::District");
Tab::Permission->has_a(tourn   => "Tab::Tourn");
Tab::Permission->has_a(region  => "Tab::Region");
Tab::Permission->has_a(category   => "Tab::Category");
Tab::Permission->has_a(chapter => "Tab::Chapter");
Tab::Permission->has_a(person => "Tab::Person");

__PACKAGE__->_register_datetimes( qw/timestamp/);

