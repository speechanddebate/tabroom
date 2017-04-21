package Tab::NSDA::NationalsInstance;
use base 'Tab::NSDA::NationalsDBI';
Tab::NSDA::NationalsInstance->table('up_instance');
Tab::NSDA::NationalsInstance->columns(Essential => qw/instance_id
	status tourn_id prefix sdate edate 
	tournament district_name location level caliplan 
	bonus nBonus accel tstamp
/);

__PACKAGE__->_register_dates( qw/sdate edate tstamp/);

