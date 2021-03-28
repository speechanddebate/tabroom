package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name child priority truncate truncate_smallest
									count multiplier violation tiebreak_set
									highlow highlow_count highlow_threshold count_round
									timestamp
								/);
Tab::Tiebreak->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::Tiebreak->has_a(child => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

