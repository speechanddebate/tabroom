package Tab::Dues;
use base 'Tab::DBI';
Tab::Dues->table('due_payment');
Tab::Dues->columns(All => qw/id timestamp chapter amount paid_on circuit/);
Tab::Dues->has_a(chapter => 'Tab::Chapter');
Tab::Dues->has_a(circuit => 'Tab::Circuit');
__PACKAGE__->_register_datetimes( qw/paid_on/);
