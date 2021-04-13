package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name
									count count_round
									truncate truncate_smallest
									multiplier violation result priority
									highlow highlow_count highlow_threshold
									child tiebreak_set
									timestamp/);
Tab::Tiebreak->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::Tiebreak->has_a(child => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

