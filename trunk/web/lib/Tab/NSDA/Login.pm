package Tab::NSDA::Login;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Login->table('logins');
Tab::NSDA::Login->columns(Essential => qw/login_id username password person_id name/);

