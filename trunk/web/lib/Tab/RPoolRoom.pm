package Tab::RPoolRoom;
use base 'Tab::DBI';
Tab::RPoolRoom->table('rpool_room');
Tab::RPoolRoom->columns(All => qw/id room rpool timestamp/);
Tab::RPoolRoom->has_a(room => 'Tab::Room');
Tab::RPoolRoom->has_a(rpool => 'Tab::RPool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
