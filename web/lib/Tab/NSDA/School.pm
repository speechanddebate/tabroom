package Tab::NSDA::School;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::School->table('auto_schools');
Tab::NSDA::School->columns(Essential => qw/school_id instance_id state city name nfl_school_id alt_id/);




