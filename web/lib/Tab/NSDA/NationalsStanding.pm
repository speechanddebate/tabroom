package Tab::NSDA::NationalsStanding;
use base 'Tab::NSDA::NationalsDBI';
Tab::NSDA::NationalsStanding->table('final_standings');

Tab::NSDA::NationalsStanding->columns(Essential => qw/ 
	district_id src_instance_id abbr place seli school_id 
	ualt_id partner_ualt_id instance_event_id skip 
/);

