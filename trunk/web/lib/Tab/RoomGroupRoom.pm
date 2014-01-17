package Tab::RoomGroupRoom;
use base 'Tab::DBI';
Tab::RoomGroupRoom->table('room_group_room');
Tab::RoomGroupRoom->columns(All => qw/id room room_group timestamp/);
Tab::RoomGroupRoom->has_a(room => 'Tab::Room');

__PACKAGE__->_register_datetimes( qw/timestamp/);
