package Tab::NSDA::DistrictDate;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::DistrictDate->table('DT_DATES');

Tab::NSDA::DistrictDate->columns(Essential => qw/
	dt_dates_id dist_id weekend sdate edate 
	hi di duo ix dx oo cx ld pf cd poi inf bq 
 	year web tab location jot_tourn_id 
		start_time end_time cell_phone 
		weekend_id
/);

__PACKAGE__->_register_dates( qw/sdate edate/);

