package Tab::FollowJudge;
use base 'Tab::DBI';
Tab::FollowJudge->table('follow_judge');
Tab::FollowJudge->columns(Primary => qw/id/);
Tab::FollowJudge->columns(Essential => qw/judge cell email domain timestamp follower/);
Tab::FollowJudge->has_a(judge => 'Tab::Judge');
Tab::FollowJudge->has_a(follower => 'Tab::Account');
__PACKAGE__->_register_datetimes( qw/timestamp/);
