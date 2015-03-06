package Tab::BallotValue;
use base 'Tab::DBI';
Tab::BallotValue->table('ballot_value');
Tab::BallotValue->columns(Primary => qw/id/);
Tab::BallotValue->columns(Essential => qw/ballot tag student value/);
Tab::BallotValue->columns(Others => qw/timestamp content cat_id tiebreak position/);
Tab::BallotValue->columns(TEMP => qw/panelid entryid roundtype roundid studentid judgeid bye ballotid chair pullup/);

Tab::BallotValue->has_a(ballot => 'Tab::Ballot');
Tab::BallotValue->has_a(student => 'Tab::Student');

__PACKAGE__->_register_datetimes( qw/timestamp/);

