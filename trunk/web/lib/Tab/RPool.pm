package Tab::RPool;
use base 'Tab::DBI';
Tab::RPool->table('rpool');
Tab::RPool->columns(All => qw/id name tourn timestamp/);
Tab::RPool->has_many(rounds => [ Tab::RPoolRound => 'round'], 'rpool');
Tab::RPool->has_many(rooms => [ Tab::RPoolRoom => 'room'], 'rpool');
Tab::RPool->has_many(rpools => 'Tab::RPoolRoom','rpool');
Tab::RPool->has_many(round_links => 'Tab::RPoolRound','rpool');
Tab::RPool->has_many(room_links => 'Tab::RPoolRoom','rpool');

__PACKAGE__->_register_datetimes( qw/timestamp/);
