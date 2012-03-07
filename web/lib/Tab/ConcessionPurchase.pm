package Tab::ConcessionPurchase;
use base 'Tab::DBI';
Tab::ConcessionPurchase->table('purchase');
Tab::ConcessionPurchase->columns(Essential => qw/id concession quantity school timestamp/);
Tab::ConcessionPurchase->has_a(school => 'Tab::School');
Tab::ConcessionPurchase->has_a(concession => 'Tab::Concession');
__PACKAGE__->_register_datetimes( qw/timestamp/);
