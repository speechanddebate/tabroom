package Tab::RoomGroup;
use base 'Tab::DBI';
Tab::RoomGroup->table('room_group');
Tab::RoomGroup->columns(All => qw/id name tourn timestamp/);
__PACKAGE__->_register_datetimes( qw/timestamp/);

