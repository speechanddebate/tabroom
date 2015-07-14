package Tab::RPoolRound;
use base 'Tab::DBI';
Tab::RPoolRound->table('rpool_round');
Tab::RPoolRound->columns(All => qw/id round rpool timestamp/);
Tab::RPoolRound->has_a(round => 'Tab::Round');
Tab::RPoolRound->has_a(rpool => 'Tab::RPool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
