package Tab::NSDA::DistrictLeaders;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::District->table('nfl.DISTRICT_LEADERSHIP');
Tab::NSDA::District->columns(Essential => qw/district_id year slot user_id position/);

