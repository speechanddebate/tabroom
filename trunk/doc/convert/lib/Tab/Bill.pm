

package Tab::Bill;
use base 'Tab::DBI';
Tab::Bill->table('bill');
Tab::Bill->columns(All => qw/id chapter title file approved tournament submitted timestamp/);
Tab::Bill->has_a(chapter => 'Tab::Chapter');
Tab::Bill->has_a(tournament => 'Tab::Tournament');
__PACKAGE__->_register_datetimes( qw/submitted/);



