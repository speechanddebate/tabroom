package Tab::NSDA::MundtDetail;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::MundtDetail->table('mundt_detail');
Tab::NSDA::MundtDetail->columns(Essential => qw/detail_id DistID DistName 
		SchoolName SchoolState 
		CharterType CompetitorCode CompetitorName 
		Rounds tourn_year nfl_ualt_id nfl_school_id
	/);
Tab::NSDA::MundtDetail->has_a(nfl_school_id => 'Tab::NSDA::School');
Tab::NSDA::MundtDetail->has_a(nfl_ualt_id => 'Tab::NSDA::Person');
