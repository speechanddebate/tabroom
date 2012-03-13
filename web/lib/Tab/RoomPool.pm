package Tab::RoomPool;
use base 'Tab::DBI';
Tab::RoomPool->table('room_pool');
Tab::RoomPool->columns(All => qw/id room reserved event timestamp/);

Tab::RoomPool->has_a(event => 'Tab::Event');
Tab::RoomPool->has_a(room => 'Tab::Room');

__PACKAGE__->_register_datetimes( qw/timestamp/);

sub tourn { 
    my $self = shift;
    return Tab::Tourn->search_by_room_pool($self->id);
}
