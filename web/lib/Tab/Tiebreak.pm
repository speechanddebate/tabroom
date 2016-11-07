package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name child priority count multiplier tiebreak_set timestamp highlow highlow_count/);
Tab::Tiebreak->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::Tiebreak->has_a(child => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

