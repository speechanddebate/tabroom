package Tab::FollowSchool;
use base 'Tab::DBI';
Tab::FollowSchool->table('follow_school');
Tab::FollowSchool->columns(Primary => qw/id/);
Tab::FollowSchool->columns(Essential => qw/school event follower timestamp/);
Tab::FollowSchool->has_a(school => 'Tab::School');
Tab::FollowSchool->has_a(event => 'Tab::School');
Tab::FollowSchool->has_a(follower => 'Tab::Account');
__PACKAGE__->_register_datetimes( qw/timestamp/);
