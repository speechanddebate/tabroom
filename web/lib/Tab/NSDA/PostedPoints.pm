package Tab::NSDA::PostedPoints;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::PostedPoints->table('NEW_POINTS');
Tab::NSDA::PostedPoints->columns(Essential => qw/
		point_id 
		student_id coach_id 
		event_info event_cat_id event_sub_cat_id 
		wins losses nodecisions ranks points
		startdate enddate 
		event_details 
		description 
		tstamp isAutoPost serv_type_id/);


__PACKAGE__->_register_dates( qw/startdate enddate/);
