package Tab::EntryStudent;
use base 'Tab::DBI';
Tab::EntryStudent->table('entry_student');
Tab::EntryStudent->columns(All => qw/id entry timestamp student/);
Tab::EntryStudent->has_a(student => 'Tab::Student');
Tab::EntryStudent->has_a(entry => 'Tab::Entry');

