package Tab::ConcessionPurchaseOption;
use base 'Tab::DBI';
Tab::ConcessionPurchaseOption->table('concession_purchase_option');
Tab::ConcessionPurchaseOption->columns(All => qw/id concession_purchase concession_option timestamp/);

Tab::ConcessionPurchaseOption->has_a(concession_option => 'Tab::ConcessionOption');
Tab::ConcessionPurchaseOption->has_a(concession_purchase => 'Tab::ConcessionPurchase');
