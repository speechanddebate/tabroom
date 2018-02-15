package Tab::Invoice;
use base 'Tab::DBI';
Tab::Invoice->table('fine');
Tab::Invoice->columns(All => qw/id nsda_invoice paid total details school timestamp/);

Tab::Invoice->has_a(school => 'Tab::School');
__PACKAGE__->_register_datetimes( qw/timestamp/);
