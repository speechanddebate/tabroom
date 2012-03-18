package Tab::PoolGroup;
use base 'Tab::DBI';
Tab::PoolGroup->table('pool_group');
Tab::PoolGroup->columns(Primary => qw/id/);
Tab::PoolGroup->columns(Essential => qw/judge_group pool/);
Tab::PoolGroup->has_a(pool => "Tab::Pool");
Tab::PoolGroup->has_a(judge_group => "Tab::JudgeGroup");


