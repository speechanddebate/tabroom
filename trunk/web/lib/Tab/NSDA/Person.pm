package Tab::NSDA::Person;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Person->table('NEW_USERS');
Tab::NSDA::Person->columns(Essential => qw/ualt_id user_id ufname umname ulname utype grad_yr graduated total_pts/);




