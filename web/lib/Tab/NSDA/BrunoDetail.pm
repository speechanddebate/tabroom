package Tab::NSDA::BrunoDetail;
use base 'Tab::NSDA::PointsDBI';
Tab::NSDA::BrunoDetail->table('bruno_detail');
Tab::NSDA::BrunoDetail->columns(Essential => qw/detail_id DistID DistName 
		SchoolName SchoolState 
		CharterType CompetitorCode CompetitorName 
		Rounds tourn_year nfl_ualt_id nfl_school_id
	/);
Tab::NSDA::BrunoDetail->has_a(nfl_school_id => 'Tab::NSDA::School');
Tab::NSDA::BrunoDetail->has_a(nfl_ualt_id => 'Tab::NSDA::Person');
