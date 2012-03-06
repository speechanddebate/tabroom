package Tab::Purchase;
use base 'Tab::DBI';
Tab::Purchase->table('purchase');
Tab::Purchase->columns(Essential => qw/id item quantity school timestamp/);
Tab::Purchase->has_a(school => 'Tab::School');
Tab::Purchase->has_a(item => 'Tab::Item');
__PACKAGE__->_register_datetimes( qw/timestamp/);
