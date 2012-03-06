package Tab::TiebreakSet;
use base 'Tab::DBI';
Tab::TiebreakSet->table('tiebreak_set');
Tab::TiebreakSet->columns(All => qw/id tourn name timestamp/);
Tab::TiebreakSet->has_a(tourn => 'Tab::Tourn');
Tab::TiebreakSet->has_many(tiebreaks => 'Tab::Tiebreak', 'tb_set');

__PACKAGE__->_register_datetimes( qw/timestamp/);

