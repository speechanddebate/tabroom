package Tab::StudentVote;
use base 'Tab::DBI';
Tab::StudentVote->table('student_vote');
Tab::StudentVote->columns(Primary => qw/id/);
Tab::StudentVote->columns(Essential => qw/rank panel entry voter entered_by entered_at timestamp/);

Tab::StudentVote->has_a(entry => 'Tab::Entry');
Tab::StudentVote->has_a(voter => 'Tab::Entry');
Tab::StudentVote->has_a(panel => 'Tab::Panel');
Tab::StudentVote->has_a(entered_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/entered_at/);

