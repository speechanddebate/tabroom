package Tab::RoomStrike;
use base 'Tab::DBI';
Tab::RoomStrike->table('roomblock');
Tab::RoomStrike->columns(All => qw/id timestamp room type event timeslot special tourn/);
Tab::RoomStrike->has_a(event => 'Tab::Event');
Tab::RoomStrike->has_a(room => 'Tab::Room');
Tab::RoomStrike->has_a(timeslot => 'Tab::Timeslot');
Tab::RoomStrike->has_a(tourn => 'Tab::Tourn');
