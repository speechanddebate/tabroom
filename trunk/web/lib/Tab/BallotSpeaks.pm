package Tab::BallotSpeaks;
use base 'Tab::DBI';
Tab::BallotSpeaks->table('ballot_speaks');
Tab::BallotSpeaks->columns(Primary => qw/id/);
Tab::BallotSpeaks->columns(Essential => qw/ballot student entry points timestamp/);

Tab::BallotSpeaks->has_a(ballot => 'Tab::Ballot');
Tab::BallotSpeaks->has_a(student => 'Tab::Student');
Tab::BallotSpeaks->has_a(entry => 'Tab::Entry');

__PACKAGE__->_register_dates( qw/timestamp/);

