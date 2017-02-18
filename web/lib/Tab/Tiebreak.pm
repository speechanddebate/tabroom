package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name child priority truncate 
									count multiplier tiebreak_set 
									highlow highlow_count
									timestamp
								/);
Tab::Tiebreak->has_a(tiebreak_set => 'Tab::TiebreakSet');
Tab::Tiebreak->has_a(child => 'Tab::TiebreakSet');
Tab::Tiebreak->has_many(rounds => 'Tab::Round', 'tiebreak_set' => { order_by => 'name'}  );

__PACKAGE__->_register_datetimes( qw/timestamp/);

