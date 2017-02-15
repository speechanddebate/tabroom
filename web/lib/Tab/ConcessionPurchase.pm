package Tab::ConcessionPurchase;
use base 'Tab::DBI';
Tab::ConcessionPurchase->table('concession_purchase');
Tab::ConcessionPurchase->columns(Essential => qw/id 
		concession 
		quantity 
		school 
		placed fulfilled 
		timestamp
	/);
Tab::ConcessionPurchase->has_a(school => 'Tab::School');
Tab::ConcessionPurchase->has_a(concession => 'Tab::Concession');

Tab::ConcessionPurchase->has_many(purchase_options => 'Tab::ConcessionPurchaseOption', 'concession_purchase');

Tab::ConcessionPurchase->has_many(
	options => 
		[Tab::ConcessionPurchaseOption => 'concession_option'], 
		'concession_purchase'
	);

__PACKAGE__->_register_datetimes( qw/timestamp placed/);
