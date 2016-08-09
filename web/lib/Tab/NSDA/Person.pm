package Tab::NSDA::Person;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Person->table('NEW_USERS');
Tab::NSDA::Person->columns(Essential => qw/user_id ualt_id ufname umname ulname utype
											uemail grad_yr graduated paid_status ustatus 
											uapproved total_pts/);


