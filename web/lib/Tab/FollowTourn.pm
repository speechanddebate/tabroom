package Tab::FollowTourn;
use base 'Tab::DBI';
Tab::FollowTourn->table('follow_tourn');
Tab::FollowTourn->columns(Primary => qw/id/);
Tab::FollowTourn->columns(Essential => qw/tourn follower/);
Tab::FollowTourn->has_a(tourn => 'Tab::Tourn');
Tab::FollowTourn->has_a(follower => 'Tab::Account');
