package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name
									count count_round
									truncate truncate_smallest
									multiplier violation result priority
									highlow highlow_count highlow_threshold
									highlow_target child protocol chair
									timestamp/);
Tab::Tiebreak->has_a(protocol => 'Tab::Protocol');
Tab::Tiebreak->has_a(child => 'Tab::Protocol');

__PACKAGE__->_register_datetimes( qw/timestamp/);

