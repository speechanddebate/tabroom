package Tab::RoomGroupRound;
use base 'Tab::DBI';
Tab::RoomGroupRound->table('room_group_round');
Tab::RoomGroupRound->columns(All => qw/id round room_group timestamp/);
Tab::RoomGroupRound->has_a(round => 'Tab::Round');

__PACKAGE__->_register_datetimes( qw/timestamp/);
