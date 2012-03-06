package Tab::Housing;
use base 'Tab::DBI';
Tab::Housing->table('housing');
Tab::Housing->columns(Primary => "id");
Tab::Housing->columns(Essential => qw/type requested timestamp judge student waitlist tourn/);
Tab::Housing->columns(Other => qw/night/);
Tab::Housing->columns(TEMP => qw/name school/);
Tab::Housing->has_a(tourn => 'Tab::Tourn');
Tab::Housing->has_a(judge => 'Tab::Judge');
Tab::Housing->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/requested/);
__PACKAGE__->_register_dates( qw/night/);

Tab::Housing->set_sql( waitlisted => "select distinct housing.* from housing,comp
											where housing.night = ?
											and housing.waitlist = 1
											and housing.tourn = ?
											and comp.tourn = housing.tourn
											and comp.student = housing.student
											and comp.waitlist = 0");

Tab::Housing->set_sql( by_school => "select distinct housing.* from housing,comp
										where comp.school = ? 
										and comp.tourn = housing.tourn
										and (housing.student = comp.student 
										or housing.student = comp.partner)");

Tab::Housing->set_sql( judge_by_school => "select distinct housing.* from housing,judge
										where judge.school = ? 
										and judge.tourn = housing.tourn
										and housing.judge = judge.id");

Tab::Housing->set_sql( by_student =>   "select distinct housing.* from housing
										and housing.student = ?
										and housing.tourn = ? ");

Tab::Housing->set_sql( by_partcomp => "select distinct housing.* from housing,comp
										where comp.id = ? 
										and housing.student = comp.partner
										and housing.tourn = comp.tourn");
