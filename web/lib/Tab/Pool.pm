package Tab::Pool;
use base 'Tab::DBI';
Tab::Pool->table('pool');
Tab::Pool->columns(Primary => qw/id/);
Tab::Pool->columns(Essential => qw/name tourn judge_group standby standby_timeslot timestamp/);
Tab::Pool->columns(Others => qw/site publish registrant burden event_based/);

Tab::Pool->has_a(standby_timeslot => 'Tab::Timeslot');

Tab::Pool->has_a(site => 'Tab::Site');
Tab::Pool->has_a(tourn => 'Tab::Tourn');
Tab::Pool->has_a(judge_group => "Tab::JudgeGroup");

Tab::Pool->has_many(rounds => 'Tab::Round', 'pool');
Tab::Pool->has_many(pool_judges => 'Tab::PoolJudge', 'pool');

Tab::Pool->has_many(judges => [Tab::PoolJudge => 'judge']);

__PACKAGE__->_register_datetimes( qw/timestamp/);

