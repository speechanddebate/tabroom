package Tab::NSDA::District;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::District->table('NEW_DISTRICTS');

Tab::NSDA::District->columns(Essential => qw/
	dist_id dist_num dist_name 
	dist_chair dist_curr_deg dist_arch_deg
	realm dist_group dist_level
/);



