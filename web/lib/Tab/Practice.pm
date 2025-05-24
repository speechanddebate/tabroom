package Tab::Practice;
use base 'Tab::DBI';
Tab::Practice->table('practice');
Tab::Practice->columns(Essential => qw/id name tag start end reported created_at created_by chapter timestamp /);
Tab::Practice->has_a(chapter => 'Tab::Chapter');
Tab::Practice->has_a(created_by => 'Tab::Person');
Tab::Practice->has_many(students => [Tab::PracticeStudent => 'student']);
__PACKAGE__->_register_datetimes( qw/start end created_at timestamp/);

