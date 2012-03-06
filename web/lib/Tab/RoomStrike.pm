package Tab::RoomBlock;
use base 'Tab::DBI';
Tab::RoomBlock->table('roomblock');
Tab::RoomBlock->columns(All => qw/id timestamp room type event timeslot special tourn/);
Tab::RoomBlock->has_a(event => 'Tab::Event');
Tab::RoomBlock->has_a(room => 'Tab::Room');
Tab::RoomBlock->has_a(timeslot => 'Tab::Timeslot');
Tab::RoomBlock->has_a(tourn => 'Tab::Tourn');
