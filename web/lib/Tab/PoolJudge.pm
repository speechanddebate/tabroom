package Tab::PoolJudge;
use base 'Tab::DBI';
Tab::PoolJudge->table('pool_judge');
Tab::PoolJudge->columns(Primary => qw/id/);
Tab::PoolJudge->columns(Essential => qw/timestamp pool judge type/);
Tab::PoolJudge->has_a(judge => "Tab::Judge");
Tab::PoolJudge->has_a(pool => "Tab::Pool");

