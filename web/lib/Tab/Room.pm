package Tab::Room;
use base 'Tab::DBI';
Tab::Room->table('room');
Tab::Room->columns(Primary => qw/id/);
Tab::Room->columns(Essential => qw/name site inactive/);
Tab::Room->columns(Others => qw/quality timestamp capacity notes ada rowcount seats deleted url/);
Tab::Room->columns(TEMP => "score");

Tab::Room->has_a(site => 'Tab::Site');
Tab::Room->has_many(panels => 'Tab::Panel', 'room');
Tab::Room->has_many(strikes => 'Tab::RoomStrike', 'room');
Tab::Room->has_many(blocks => 'Tab::RoomStrike', 'room');
Tab::Room->has_many(rpools => [Tab::RPoolRoom => 'rpool']);

__PACKAGE__->_register_datetimes( qw/timestamp/);


