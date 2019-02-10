package Tab::Invoice;
use base 'Tab::DBI';
Tab::Invoice->table('invoice');
Tab::Invoice->columns(All => qw/id blusynergy blu_number paid total details school timestamp/);

Tab::Invoice->has_a(school => 'Tab::School');
Tab::Invoice->has_many(payments => 'Tab::Fine', "invoice");
__PACKAGE__->_register_datetimes( qw/timestamp/);
