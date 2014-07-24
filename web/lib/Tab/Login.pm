package Tab::Login;
use base 'Tab::DBI';
Tab::Login->table('room');
Tab::Login->columns(Primary => qw/id/);
Tab::Login->columns(Essential => qw/username password salt name/);
Tab::Login->columns(Others => qw/quality timestamp capacity notes building ada/);
Tab::Login->columns(TEMP => "score");

Tab::Login->has_a(site => 'Tab::Site');
Tab::Login->has_many(strikes => 'Tab::RoomStrike', 'room');

__PACKAGE__->_register_datetimes( qw/timestamp/);


