package Tab::HousingStudent;
use base 'Tab::DBI';
Tab::HousingStudent->table('housing_student');
Tab::HousingStudent->columns(Primary => "id");
Tab::HousingStudent->columns(Essential => qw/type requested timestamp judge student waitlist tourn/);
Tab::HousingStudent->columns(Other => qw/night/);
Tab::HousingStudent->columns(TEMP => qw/name school/);
Tab::HousingStudent->has_a(tourn => 'Tab::Tourn');
Tab::HousingStudent->has_a(judge => 'Tab::Judge');
Tab::HousingStudent->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/requested/);
__PACKAGE__->_register_dates( qw/night/);

Tab::HousingStudent->set_sql( waitlisted => "select distinct housing_student.* from housing_student,entry
											where housing_student.night = ?
											and housing_student.waitlist = 1
											and housing_student.tourn = ?
											and entry.tourn = housing_student.tourn
											and entry.student = housing_student.student
											and entry.waitlist = 0");

Tab::HousingStudent->set_sql( by_school => "select distinct housing_student.* from housing_student,entry
										where entry.school = ? 
										and entry.tourn = housing_student.tourn
										and (housing_student.student = entry.student 
										or housing_student.student = entry.partner)");

Tab::HousingStudent->set_sql( judge_by_school => "select distinct housing_student.* from housing_student,judge
										where judge.school = ? 
										and judge.tourn = housing_student.tourn
										and housing_student.judge = judge.id");

Tab::HousingStudent->set_sql( by_student =>   "select distinct housing_student.* from housing_student
										and housing_student.student = ?
										and housing_student.tourn = ? ");

Tab::HousingStudent->set_sql( by_partentry => "select distinct housing_student.* from housing_student,entry
										where entry.id = ? 
										and housing_student.student = entry.partner
										and housing_student.tourn = entry.tourn");
