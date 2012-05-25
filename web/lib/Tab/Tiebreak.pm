package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id name priority count multiplier tb_set timestamp highlow/);
Tab::Tiebreak->has_a(tb_set => 'Tab::TiebreakSet');

__PACKAGE__->_register_datetimes( qw/timestamp/);

