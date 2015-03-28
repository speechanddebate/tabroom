package Tab::NSDA::PersonSchool;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::PersonSchool->table('NEW_USERS_TO_SCHOOL');
Tab::NSDA::PersonSchool->columns(Essential => qw/ualt_id school_id startdate enddate tostrength hasaccess hasaccessdate accessgranted isadmin/);

Tab::NSDA::PersonSchool->has_a( school_id => "Tab::NSDA::MemberSchool");

__PACKAGE__->_register_dates( qw/startdate enddate hasaccessdate/);

