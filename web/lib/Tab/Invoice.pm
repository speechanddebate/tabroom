package Tab::Invoice;
use base 'Tab::DBI';
Tab::Invoice->table('invoice');
Tab::Invoice->columns(All => qw/id blusynergy paid total details school timestamp/);

Tab::Invoice->has_a(school => 'Tab::School');
__PACKAGE__->_register_datetimes( qw/timestamp/);
