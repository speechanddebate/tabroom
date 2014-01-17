package Tab::RoundGroupRound;
use base 'Tab::DBI';
Tab::RoundGroupRound->table('room_group_round');
Tab::RoundGroupRound->columns(All => qw/id round round_group timestamp/);
Tab::RoundGroupRound->has_a(round => 'Tab::Round');

__PACKAGE__->_register_datetimes( qw/timestamp/);
