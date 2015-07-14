package Tab::JPoolRound;
use base 'Tab::DBI';
Tab::JPoolRound->table('jpool_round');
Tab::JPoolRound->columns(All => qw/id round jpool timestamp/);
Tab::JPoolRound->has_a(round => 'Tab::Round');
Tab::JPoolRound->has_a(jpool => 'Tab::JPool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
