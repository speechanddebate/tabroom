package Tab::NSDA::Points;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Points->table('AUTO_POINTS_TRANSFER');
Tab::NSDA::Points->columns(Essential => qw/apt_id instance_id nfl_school_id nfl_student_id round result event_cat_id event_sub_cat_id alt_event_id points nfl_coach_id point_id type/);

