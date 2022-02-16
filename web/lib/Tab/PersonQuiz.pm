package Tab::PersonQuiz;
use base 'Tab::DBI';
Tab::PersonQuiz->table('person_quiz');
Tab::PersonQuiz->columns(All => qw/id pending hidden approved_by updated_at completed answers person quiz timestamp/);

Tab::PersonQuiz->has_a(quiz => 'Tab::Quiz');
Tab::PersonQuiz->has_a(person => 'Tab::Person');
Tab::PersonQuiz->has_a(approved_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp updated_at/);

