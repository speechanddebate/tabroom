package Tab::RoomGroup;
use base 'Tab::DBI';
Tab::RoomGroup->table('room_group');
Tab::RoomGroup->columns(All => qw/id name tourn timestamp/);
Tab::RoomGroup->has_many(rounds => [ Tab::RoomGroupRound => 'round'], 'room_group');
Tab::RoomGroup->has_many(rooms => [ Tab::RoomGroupRoom => 'room'], 'room_group');
Tab::RoomGroup->has_many(room_pools => 'Tab::RoomGroupRoom','room_group');

__PACKAGE__->_register_datetimes( qw/timestamp/);
