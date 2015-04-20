package Tab::NSDA::Points;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::Points->table('auto_points');
Tab::NSDA::Points->columns(Essential => qw/instance_id id type round result event_id entry_id student_id school_id name alt_entry_id alt_student_id nfl_school_id nfl_student_id/);

#APT apt_id instance_id nfl_school_id nfl_student_id round result event_cat_id event_sub_cat_id alt_event_id points nfl_coach_id point_id type/);
