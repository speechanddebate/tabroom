package Tab::Tiebreak;
use base 'Tab::DBI';
Tab::Tiebreak->table('tiebreak');
Tab::Tiebreak->columns(All => qw/id tiebreaker count multiplier priority covers tb_set timestamp method/);
Tab::Tiebreak->has_a(tb_set => 'Tab::TiebreakSet');

