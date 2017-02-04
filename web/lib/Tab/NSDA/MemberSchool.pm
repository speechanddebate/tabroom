package Tab::NSDA::MemberSchool;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::MemberSchool->table('NEW_SCHOOLS');
Tab::NSDA::MemberSchool->columns(Essential => qw/school_id school_name school_addr 
										school_city school_state school_zip realm 
										school_total_deg 
										school_paid_status 
										school_charter_status
										school_trophy_points
								/);


