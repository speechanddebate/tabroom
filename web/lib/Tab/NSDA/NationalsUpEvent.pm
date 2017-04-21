package Tab::NSDA::NationalsUpEvent;
use base 'Tab::NSDA::NationalsDBI';
Tab::NSDA::NationalsUpEvent->table('up_events');
Tab::NSDA::NationalsUpEvent->columns(Essential => qw/
	web_event_id event_id instance_id abbr name 
	isdebate iscongress nRounds nEntries nAdvancing
/);

