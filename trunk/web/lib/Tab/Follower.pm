package Tab::Follower;
use base 'Tab::DBI';
Tab::Follower->table('follower');
Tab::Follower->columns(Primary => qw/id/);
Tab::Follower->columns(Essential => qw/cell email domain follower type entry judge school tourn account timestamp/);
Tab::Follower->has_a(entry => 'Tab::Entry');
Tab::Follower->has_a(school => 'Tab::School');
Tab::Follower->has_a(judge => 'Tab::Judge');
Tab::Follower->has_a(tourn => 'Tab::Tourn');
Tab::Follower->has_a(account => 'Tab::Account');
Tab::Follower->has_a(follower => 'Tab::Account');

__PACKAGE__->_register_datetimes( qw/timestamp/);
