package Tab::Follower;
use base 'Tab::DBI';
Tab::Follower->table('follower');
Tab::Follower->columns(Primary => qw/id/);
Tab::Follower->columns(Essential => qw/type cell email domain follower tourn judge entry school person timestamp/);
Tab::Follower->has_a(follower => 'Tab::Person');
Tab::Follower->has_a(tourn => 'Tab::Tourn');
Tab::Follower->has_a(judge => 'Tab::Judge');
Tab::Follower->has_a(entry => 'Tab::Entry');
Tab::Follower->has_a(school => 'Tab::School');
Tab::Follower->has_a(person => 'Tab::Person');

__PACKAGE__->_register_datetimes( qw/timestamp/);
