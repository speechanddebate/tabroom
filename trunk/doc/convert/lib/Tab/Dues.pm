package Tab::Dues;
use base 'Tab::DBI';
Tab::Dues->table('due_payment');
Tab::Dues->columns(All => qw/id timestamp chapter amount paid_on league/);
Tab::Dues->has_a(chapter => 'Tab::Chapter');
Tab::Dues->has_a(league => 'Tab::League');
__PACKAGE__->_register_datetimes( qw/paid_on/);
