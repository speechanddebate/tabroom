

package Tab::Bill;
use base 'Tab::DBI';
Tab::Bill->table('bill');
Tab::Bill->columns(All => qw/id chapter title file approved tourn submitted timestamp/);
Tab::Bill->has_a(chapter => 'Tab::Chapter');
Tab::Bill->has_a(tourn => 'Tab::Tourn');
__PACKAGE__->_register_datetimes( qw/submitted/);



