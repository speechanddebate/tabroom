package Tab::RoomPool;
use base 'Tab::DBI';
Tab::RoomPool->table('room_pool');
Tab::RoomPool->columns(All => qw/id room reserved event tourn/);
Tab::RoomPool->has_a(event => 'Tab::Event');
Tab::RoomPool->has_a(room => 'Tab::Room');
Tab::RoomPool->has_a(tourn => 'Tab::Tourn');

Tab::RoomPool->set_sql(by_tourn => "
							select distinct room_pool.*
							from room_pool,event 
							where event.tourn=?
							and room_pool.event = event.id");



