package Tab::StudentBallot;
use base 'Tab::DBI';
Tab::StudentBallot->table('student_ballot');
Tab::StudentBallot->columns(Primary => qw/id/);
Tab::StudentBallot->columns(Essential => qw/tag panel entry value/);
Tab::StudentBallot->columns(Others => qw/voter entered_by timestamp/);

Tab::StudentBallot->has_a(panel => 'Tab::Panel');
Tab::StudentBallot->has_a(entry => 'Tab::Entry');
Tab::StudentBallot->has_a(voter => 'Tab::Entry');

Tab::Ballot->has_a(entered_by => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp/);

