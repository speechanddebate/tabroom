package Tab::NSDA::NationalsEvent;
use base 'Tab::NSDA::NationalsDBI';
Tab::NSDA::NationalsEvent->table('nats_events');

Tab::NSDA::NationalsEvent->columns(Essential => qw/
	event_id tourn_id name abbr2 type cat_id sub_cat_id
/);

