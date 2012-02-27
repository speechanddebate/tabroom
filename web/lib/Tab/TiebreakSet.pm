package Tab::TiebreakSet;
use base 'Tab::DBI';
Tab::TiebreakSet->table('tiebreak_set');
Tab::TiebreakSet->columns(All => qw/id tournament name timestamp/);
Tab::TiebreakSet->has_a(tournament => 'Tab::Tournament');
Tab::TiebreakSet->has_many(tiebreaks => 'Tab::Tiebreak', 'tb_set');

__PACKAGE__->_register_datetimes( qw/timestamp/);

