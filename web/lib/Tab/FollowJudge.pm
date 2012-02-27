package Tab::FollowJudge;
use base 'Tab::DBI';
Tab::FollowJudge->table('follow_judge');
Tab::FollowJudge->columns(Primary => qw/id/);
Tab::FollowJudge->columns(Essential => qw/judge cell email domain/);
Tab::FollowJudge->has_a(judge => 'Tab::Judge');

