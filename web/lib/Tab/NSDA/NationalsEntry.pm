package Tab::NSDA::NationalsEntry;
use base 'Tab::NSDA::NationalsDBI';
Tab::NSDA::NationalsEntry->table('eligible_entries');

Tab::NSDA::NationalsEntry->columns(Essential => qw/
	eligible_id tourn_id event_id district_id 
		school_id student_id partner_id 
		auto_qual isAlternate alt_date
/);

