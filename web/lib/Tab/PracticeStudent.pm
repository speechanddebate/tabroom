package Tab::PracticeStudent;
use base 'Tab::DBI';
Tab::PracticeStudent->table('practice_student');
Tab::PracticeStudent->columns(Essential => qw/id practice student created_at created_by timestamp /);
Tab::PracticeStudent->has_a(practice => 'Tab::Practice');
Tab::PracticeStudent->has_a(student => 'Tab::Student');
Tab::PracticeStudent->has_a(created_by => 'Tab::Person');
__PACKAGE__->_register_datetimes( qw/created_at timestamp/);

