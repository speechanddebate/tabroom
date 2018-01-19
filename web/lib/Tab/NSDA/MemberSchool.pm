package Tab::NSDA::MemberSchool;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::MemberSchool->table('NEW_SCHOOLS');
Tab::NSDA::MemberSchool->columns(Essential => qw/school_id school_name 
			realm 
			school_addr school_addr2
			school_city school_state school_zip 
			school_phone
			school_total_deg 
			school_status
			school_paid_status 
			school_charter_status
			school_trophy_points
/);


