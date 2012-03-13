package Tab::RoomStrike;
use base 'Tab::DBI';
Tab::RoomStrike->table('roomblock');
Tab::RoomStrike->columns(All => qw/id room type event timeslot special tourn start end timestamp/);

Tab::RoomStrike->has_a(event => 'Tab::Event');
Tab::RoomStrike->has_a(room => 'Tab::Room');
Tab::RoomStrike->has_a(timeslot => 'Tab::Timeslot');
Tab::RoomStrike->has_a(tourn => 'Tab::Tourn');

__PACKAGE__->_register_datetimes( qw/timestamp/);
__PACKAGE__->_register_datetimes( qw/start end/);

