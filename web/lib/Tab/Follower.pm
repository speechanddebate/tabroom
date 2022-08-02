package Tab::Follower;
use base 'Tab::DBI';
Tab::Follower->table('follower');
Tab::Follower->columns(Primary => qw/id/);
Tab::Follower->columns(Essential => qw/type person tourn judge entry school student/);
Tab::Follower->columns(Others => qw/timestamp/);
Tab::Follower->has_a(tourn => 'Tab::Tourn');
Tab::Follower->has_a(judge => 'Tab::Judge');
Tab::Follower->has_a(entry => 'Tab::Entry');
Tab::Follower->has_a(school => 'Tab::School');
Tab::Follower->has_a(student => 'Tab::Student');
Tab::Follower->has_a(person => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp/);
