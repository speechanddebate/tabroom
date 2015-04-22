package Tab::NSDA::AltStudent;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::AltStudent->table('alt_students');
Tab::NSDA::AltStudent->columns(Essential => qw/alt_id source nfl_student_id nfl_school_id name/);

