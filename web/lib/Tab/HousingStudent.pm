package Tab::HousingStudent;
use base 'Tab::DBI';
Tab::HousingStudent->table('housing_student');
Tab::HousingStudent->columns(Primary => "id");
Tab::HousingStudent->columns(Essential => qw/tourn night type account judge student waitlist requested timestamp/);

Tab::HousingStudent->has_a(tourn => 'Tab::Tourn');
Tab::HousingStudent->has_a(account => 'Tab::Account');
Tab::HousingStudent->has_a(judge => 'Tab::Judge');
Tab::HousingStudent->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/requested/);
__PACKAGE__->_register_dates( qw/night/);

