package Tab::NSDA::MemberSchool;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::MemberSchool->table('NEW_SCHOOLS');
Tab::NSDA::MemberSchool->columns(Essential => qw/school_id school_name realm school_city school_state/);


