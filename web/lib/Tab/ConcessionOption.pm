package Tab::ConcessionOption;
use base 'Tab::DBI';
Tab::ConcessionOption->table('concession_option');
Tab::ConcessionOption->columns(Essential => qw/id concession_type name description disabled timestamp/);
Tab::ConcessionOption->has_a(concession_type => 'Tab::ConcessionType');

Tab::ConcessionOption->has_many(purchase_options => 'Tab::ConcessionPurchaseOption', 'concession_option');

Tab::ConcessionOption->has_many(purchases => [Tab::ConcessionPurchaseOption => 'student'], 'concession_option');

__PACKAGE__->_register_datetimes( qw/timestamp/);
