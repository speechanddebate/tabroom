package Tab::Pool;
use base 'Tab::DBI';
Tab::Pool->table('pool');
Tab::Pool->columns(Primary => qw/id/);
Tab::Pool->columns(Essential => qw/timestamp name judge_group standby timeslot/);
Tab::Pool->columns(Others => qw/site publish registrant burden/);

Tab::Pool->has_a(site => 'Tab::Site');
Tab::Pool->has_a(judge_group => "Tab::JudgeGroup");

Tab::Pool->has_many(rounds => 'Tab::Round', 'pool');
Tab::Pool->has_many(pool_judges => 'Tab::PoolJudge', 'pool');


Tab::Pool->set_sql(by_judge=> qq{select distinct judge.* 
						from judge,pool_judge
						where judge.id = pool_judge.judge
						and pool_judge.pool = ?});

Tab::Pool->set_sql(by_standby_judge => "select distinct pool.id
                                        from pool_judge,pool
                                        where pool_judge.judge = ?
                                        and pool_judge.pool = pool.id
                                        and pool.standby = 1");

Tab::Pool->set_sql(by_tournament => "select distinct pool.* from pool,judge_group
										where pool.judge_group = judge_group.id
										and judge_group.tournament = ?");

Tab::Pool->set_sql(judge_and_timeslot => "select distinct pool.*
                                        from pool,pool_judge
                                        where pool_judge.judge= ? 
                                        and pool_judge.pool = pool.id
                                        and pool.timeslot = ?" );

sub judges {
    my $self = shift;
	my @judges = Tab::Judge->search_bypool($self->id);
	push (@judges, Tab::Judge->search(prelim_pool => $self->id));
	return @judges;
}

